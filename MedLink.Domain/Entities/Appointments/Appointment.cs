using MedLink.Domain.Common;
using MedLink.Domain.Entities.Medical;
using MedLink.Domain.Enums;

namespace MedLink.Domain.Entities.Appointments;

public class Appointment : BaseEntity
{
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;

    public int AvailabilityId { get; set; }
    public DoctorAvailability Availability { get; set; } = null!;

    public AppointmentStatus Status { get; set; }
    public decimal Fee { get; set; }
    public string? Notes { get; set; }
    public string? CancelledReason { get; set; }
}
