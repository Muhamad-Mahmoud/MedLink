using MedLink.Domain.Entities.User;
using MedLink_Application.DTOs.UserProfile;
using MedLink_Application.Interfaces.Persistence;
using MedLink_Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProfileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<EditProfileDto> GetMyProfileAsync(
        string userId,
        string email,
        string phoneNumber)
        {
            var profile = await _unitOfWork
                .Repository<UserProfile>()
                .FindAsync(x => x.UserId == userId);

            if (profile == null)
                throw new Exception("User profile not found");

            return new EditProfileDto
            {
                FullName = profile.FullName,
                ImageUrl = profile.ImageUrl,
                Email = email,
                PhoneNumber = phoneNumber
            };
        }

        public async Task UpdateMyProfileAsync(string userId, UpdateProfileDto dto)
        {
            var profile = await _unitOfWork
                .Repository<UserProfile>()
                .FindAsync(x => x.UserId == userId);

            if (profile == null)
                throw new Exception("User profile not found");

            profile.FullName = dto.FullName;

            _unitOfWork.Repository<UserProfile>().Update(profile);
            await _unitOfWork.Complete();
        }

        public async Task UpdateProfileImageAsync(string userId, string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException("ImageUrl is required");

            var profile = await _unitOfWork
                .Repository<UserProfile>()
                .FindAsync(x => x.UserId == userId);

            if (profile == null)
                throw new Exception("User profile not found");

            profile.ImageUrl = imageUrl;

            _unitOfWork.Repository<UserProfile>().Update(profile);
            await _unitOfWork.Complete();
        }
        public async Task RemoveProfileImageAsync(string userId)
        {
            var profile = await _unitOfWork
                .Repository<UserProfile>()
                .FindAsync(x => x.UserId == userId);

            if (profile == null)
                throw new Exception("User profile not found");

            profile.ImageUrl = null;

            _unitOfWork.Repository<UserProfile>().Update(profile);
            await _unitOfWork.Complete();
        }

    }
}
