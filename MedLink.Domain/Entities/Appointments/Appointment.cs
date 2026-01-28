using MedLink.Domain.Common;
using MedLink.Domain.Entities.Medical;
using MedLink.Domain.Enums;

namespace MedLink.Domain.Entities.Appointments;

public class Appointment : BaseEntity
{
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public string BookedByUserId { get; set; } = string.Empty;

    public int ScheduleId { get; set; }
    public DoctorAvailability Schedule { get; set; } = null!;

    public string PatientName { get; set; } = string.Empty;
    public string PatientPhone { get; set; } = string.Empty;


    public AppointmentStatus Status { get; set; }
    public decimal Fee { get; set; }

}
