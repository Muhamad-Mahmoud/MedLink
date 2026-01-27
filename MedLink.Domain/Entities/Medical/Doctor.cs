using MedLink.Domain.Common;
using MedLink.Domain.Entities.Appointments;
using MedLink.Domain.Entities.User;

namespace MedLink.Domain.Entities.Medical;

public class Doctor : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;

    public int SpecialtyId { get; set; }
    public Specialization Specialization { get; set; } = null!;

    public string? Bio { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public string? ClinicDetails { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<DoctorAvailability> Availabilities { get; set; } = new List<DoctorAvailability>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
