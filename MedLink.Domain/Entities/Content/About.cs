using MedLink.Domain.Common;

namespace MedLink.Domain.Entities.Content;

public class About : BaseEntity
{
    public string Content { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
