using MedLink.Application.DTOs.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Interfaces.Services
{
    public interface IProfileAppointmentService
    {
        Task<PagedResult<AppointmentListItemDto>> GetPastAsync(string userId, int page, int pageSize);

        Task<PagedResult<AppointmentListItemDto>> GetUpcomingAsync(string userId, int page, int pageSize);

        //Task CancelAsync(
        //    string userId,
        //    int appointmentId,
        //    string? reason
        //);
    }


}
