using Microsoft.AspNetCore.Identity;

namespace MedLink.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}
