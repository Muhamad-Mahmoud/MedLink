using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedLink.Domain.Enums;

namespace MedLink.Domain.Entities.User
{
    public class UserProfile
    {
		public Gender Gender { get; set; }
		public string? MedicalHistory { get; set; }
	}
}
