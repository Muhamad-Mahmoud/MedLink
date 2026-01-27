using MedLink.Domain.Common;

namespace MedLink.Domain.Entities.Chat;

public class Message : BaseEntity
{
    public int ChatRoomId { get; set; }
    public ChatRoom ChatRoom { get; set; } = null!;

    public string SenderUserId { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
