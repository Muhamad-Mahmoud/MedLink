using MedLink.Domain.Common;
using MedLink.Domain.Entities.Medical;

namespace MedLink.Domain.Entities.User;

public class Favorite : BaseEntity
{
    public string UserId { get; set; } = string.Empty;

    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; } = null!;
}
