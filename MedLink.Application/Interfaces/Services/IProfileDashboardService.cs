using MedLink_Application.DTOs.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Interfaces.Services
{
    public interface IProfileDashboardService
    {
        Task<UserProfileDashboardDto> GetMyDashboardAsync(string userId);

    }
}
