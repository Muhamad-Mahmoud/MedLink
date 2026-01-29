using AutoMapper;
using MedLink.Domain.Identity;
using MedLink_Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Mapping
{
    public class AuthMappingProfiles : Profile
    {
        public AuthMappingProfiles()
        {
            CreateMap<RegisterModel, ApplicationUser>()
             .ForMember(d => d.UserName, o => o.MapFrom<EmailToUsernameResolver>())
             .ForMember(d => d.Email, o => o.MapFrom(s => s.Email.Contains("@") ? s.Email : null));
        }
    }
}
