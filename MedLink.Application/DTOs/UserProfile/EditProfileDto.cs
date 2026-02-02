using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.DTOs.UserProfile
{
    public class EditProfileDto
    {
        public string FullName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }

        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

}
