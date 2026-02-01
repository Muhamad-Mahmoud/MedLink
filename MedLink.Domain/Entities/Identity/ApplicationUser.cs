using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MedLink.Domain.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
}
