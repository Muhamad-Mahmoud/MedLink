using MediatR;
using MedLink.Domain.Enums;
using MedLink.Domain.Interfaces.Repositories;
using MedLink_Application.Commands;
using MedLink_Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Handlers.Commands
{

    public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand, Unit>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorAvailabilityRepository _availabilityRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IStripeService _stripeService;

        public CancelAppointmentCommandHandler(
            IAppointmentRepository appointmentRepository,
            IDoctorAvailabilityRepository availabilityRepository,
            IPaymentRepository paymentRepository,
            IStripeService stripeService)
        {
            _appointmentRepository = appointmentRepository;
            _availabilityRepository = availabilityRepository;
            _paymentRepository = paymentRepository;
            _stripeService = stripeService;
        }
        public async Task<Unit> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            
            var appointment = await _appointmentRepository.GetByIdWithDetailsAsync(request.AppointmentId);
            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with ID {request.AppointmentId} not found");

           
            if (appointment.BookedByUserId != request.CancelledByUserId)
                throw new UnauthorizedAccessException("You are not authorized to cancel this appointment");

            if (appointment.Status == AppointmentStatus.Cancelled)
                throw new InvalidOperationException("Appointment is already cancelled");

            if (appointment.Status == AppointmentStatus.Completed)
                throw new InvalidOperationException("Cannot cancel a completed appointment");

            
            appointment.Status = AppointmentStatus.Cancelled;
            appointment.CancelledReason = request.Reason;
            appointment.CancelledAt = DateTime.UtcNow;
            appointment.UpdatedAt = DateTime.UtcNow;

           
            var schedule = await _availabilityRepository.GetByIdAsync(appointment.ScheduleId);
            if (schedule != null)
            {
                schedule.IsBooked = false;
                await _availabilityRepository.UpdateAsync(schedule);
            }

            var payment = await _paymentRepository.GetByAppointmentIdAsync(appointment.Id);
            if (payment != null && payment.Status == PaymentStatus.Succeeded)
            {
                try
                {
                    var refunded = await _stripeService.RefundPaymentAsync(
                        payment.StripePaymentIntentId!,
                        payment.Amount,
                        request.Reason
                    );

                    if (refunded)
                    {
                        payment.Status = PaymentStatus.Refunded;
                        payment.RefundReason = request.Reason;
                        payment.RefundedAt = DateTime.UtcNow;
                        payment.UpdatedAt = DateTime.UtcNow;
                        await _paymentRepository.UpdateAsync(payment);
                    }
                }
                catch (Exception ex)
                {
                    // Log error but don't fail the cancellation
                    Console.WriteLine($"Refund failed: {ex.Message}");
                }
            }

            
            await _appointmentRepository.UpdateAsync(appointment);

            return Unit.Value;
        }

    }
}
