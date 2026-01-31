using MedLink_Application.DTOs.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Interfaces.Services
{
    public interface IProfileAppointmentService
    {
        Task<IReadOnlyList<AppointmentListItemDto>> GetUpcomingAsync(string userId);
        Task<IReadOnlyList<AppointmentListItemDto>> GetPastAsync(string userId);

        Task CancelAsync(
            string userId,
            int appointmentId,
            string? reason
        );
    }


}
