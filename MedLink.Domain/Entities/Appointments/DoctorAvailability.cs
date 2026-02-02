using MedLink.Domain.Common;
using MedLink.Domain.Entities.Medical;

namespace MedLink.Domain.Entities.Appointments;

public class DoctorAvailability : BaseEntity
{
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public DateTime Date { get; set; }              
    public TimeSpan StartTime { get; set; }        
    public TimeSpan EndTime { get; set; }           

    public bool IsBooked { get; set; } = false;     

    // Navigation
    public Appointment? Appointment { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
