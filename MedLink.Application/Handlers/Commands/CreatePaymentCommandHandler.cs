using AutoMapper;
using MediatR;
using MedLink.Domain.Entities.Payments;
using MedLink.Domain.Enums;
using MedLink.Domain.Interfaces.Repositories;
using MedLink_Application.Commands;
using MedLink_Application.Interfaces.Repositories;
using MedLink_Application.Responses;

namespace MedLink_Application.Handlers.Commands
{

    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, PaymentDto>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IStripeService _stripeService;
        private readonly IMapper _mapper;

        public CreatePaymentCommandHandler(
            IPaymentRepository paymentRepository,
            IAppointmentRepository appointmentRepository,
            IStripeService stripeService,
            IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _appointmentRepository = appointmentRepository;
            _stripeService = stripeService;
            _mapper = mapper;
        }

        public async Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with ID {request.AppointmentId} not found");

            if (appointment.Status != AppointmentStatus.Pending)
                throw new InvalidOperationException($"Cannot create payment for appointment with status {appointment.Status}");

            var existingPayment = await _paymentRepository.GetByAppointmentIdAsync(request.AppointmentId);
            if (existingPayment != null && existingPayment.Status == PaymentStatus.Succeeded)
                throw new InvalidOperationException("Payment already exists for this appointment");

            var (paymentIntentId, clientSecret) = await _stripeService.CreatePaymentIntentAsync(
                amount: appointment.Fee,
                currency: "EGP",
                customerEmail: request.CustomerEmail ?? appointment.PatientEmail ?? "",
                metadata: new Dictionary<string, string>
                {
            { "appointment_id", appointment.Id.ToString() },
            { "patient_name", appointment.PatientName },
            { "doctor_id", appointment.DoctorId.ToString() }
                }
            );

            var payment = new Payment
            {
                AppointmentId = request.AppointmentId,
                Amount = appointment.Fee,
                Currency = "EGP",
                Method = Enum.Parse<PaymentMethod>(request.PaymentMethod),
                Status = PaymentStatus.Pending,
                StripePaymentIntentId = paymentIntentId,
                StripeClientSecret = clientSecret,
                FailureReason = "",
                RefundReason = "",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                RefundedAt = DateTime.MinValue, // أو null لو العمود يسمح
                IsDeleted = false,
                PaidAt = null
            };

            var savedPayment = await _paymentRepository.AddAsync(payment);

            return _mapper.Map<PaymentDto>(savedPayment);
        }

    }
}
