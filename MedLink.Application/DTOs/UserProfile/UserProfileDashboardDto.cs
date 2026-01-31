using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.DTOs.UserProfile
{
    public class UserProfileDashboardDto
    {
        public ProfileHeaderDto Profile { get; set; } = null!;

        public int TotalAppointments { get; set; }
        public int UpcomingAppointments { get; set; }
        public int PrescriptionsCount { get; set; }
        public int FavoriteDoctorsCount { get; set; }
    }
}
