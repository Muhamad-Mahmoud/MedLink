using MedLink.Domain.Common;
using MedLink.Domain.Entities.Appointments;
using MedLink.Domain.Enums;

namespace MedLink.Domain.Entities.Payments;

public class Payment : BaseEntity
{
    public int AppointmentId { get; set; }
    public Appointment Appointment { get; set; } = null!;

    public string? StripePaymentIntentId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public PaymentStatus Status { get; set; }
    public DateTime? PaidAt { get; set; }
}
