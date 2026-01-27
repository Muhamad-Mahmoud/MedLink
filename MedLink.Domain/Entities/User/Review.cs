using MedLink.Domain.Common;
using MedLink.Domain.Entities.Medical;

namespace MedLink.Domain.Entities.User;

public class Review : BaseEntity
{
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;

    public int Rating { get; set; }
    public string? Comment { get; set; }
}
