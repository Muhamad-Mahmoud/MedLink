using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.DTOs.Identity
{
    public class ForgotPasswordModel
    {
        [Required, EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? ClientUri { get; set; }
    }
}
