using MedLink_Application.DTOs.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Interfaces.Services
{
        public interface IProfileService
        {
        Task CreateAsync(string userId, string fullName);
        Task<EditProfileDto> GetMyProfileAsync(
            string userId);

            Task UpdateMyProfileAsync(string userId, UpdateProfileDto dto);

            Task UpdateProfileImageAsync(string userId, string imageUrl);

            Task RemoveProfileImageAsync(string userId);

    }

}
