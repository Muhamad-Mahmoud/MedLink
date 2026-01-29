using AutoMapper;
using MedLink.Domain.Identity;
using MedLink_Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Mapping
{
    public class EmailToUsernameResolver : IValueResolver<RegisterModel, ApplicationUser, string>
    {
        public string Resolve(RegisterModel source, ApplicationUser destination, string destMember, ResolutionContext context)
        {
            var emailOrPhone = source.Email;

            if (new EmailAddressAttribute().IsValid(emailOrPhone))
                return emailOrPhone.Split('@')[0];

            return emailOrPhone;
        }
    }
}
