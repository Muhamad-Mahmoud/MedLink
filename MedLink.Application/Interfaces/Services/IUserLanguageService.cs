using MedLink.Application.DTOs.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Interfaces.Services
{
    public interface IUserLanguageService
    {
        Task<LanguageResponsetDto> GetPreferredLanguageAsync(string userId);
        Task UpdatePreferredLanguageAsync(string userId, UpdateLanguageDto dto);
    }
}
