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

    public class ConfirmPaymentCommandHandler : IRequestHandler<ConfirmPaymentCommand, Unit>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IStripeService _stripeService;

        public ConfirmPaymentCommandHandler(
            IPaymentRepository paymentRepository,
            IAppointmentRepository appointmentRepository,
            IStripeService stripeService)
        {
            _paymentRepository = paymentRepository;
            _appointmentRepository = appointmentRepository;
            _stripeService = stripeService;
        }

        public async Task<Unit> Handle(ConfirmPaymentCommand request, CancellationToken cancellationToken)
        {
          
            var payment = await _paymentRepository.GetByStripePaymentIntentIdAsync(request.PaymentIntentId);
            if (payment == null)
                throw new KeyNotFoundException($"Payment with PaymentIntentId {request.PaymentIntentId} not found");

            
            var stripeStatus = await _stripeService.GetPaymentStatusAsync(request.PaymentIntentId);

            if (stripeStatus == "succeeded")
            {
               
                payment.Status = PaymentStatus.Succeeded;
                payment.PaidAt = DateTime.UtcNow;
                payment.UpdatedAt = DateTime.UtcNow;
                await _paymentRepository.UpdateAsync(payment);

                var appointment = await _appointmentRepository.GetByIdAsync(payment.AppointmentId);
                if (appointment != null)
                {
                    appointment.Status = AppointmentStatus.Confirmed;
                    appointment.UpdatedAt = DateTime.UtcNow;
                    await _appointmentRepository.UpdateAsync(appointment);
                }
            }
            else if (stripeStatus == "canceled" || stripeStatus == "failed")
            {
                payment.Status = PaymentStatus.Failed;
                payment.FailureReason = $"Stripe status: {stripeStatus}";
                payment.UpdatedAt = DateTime.UtcNow;
                await _paymentRepository.UpdateAsync(payment);
            }

            return Unit.Value;
        }
    }

}
