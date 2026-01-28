using MedLink.Domain.Common;
using MedLink.Domain.Entities.Appointments;

namespace MedLink.Domain.Entities.Chat;

public class ChatRoom : BaseEntity
{
    public int? AppointmentId { get; set; }
    public Appointment? Appointment { get; set; }

    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
