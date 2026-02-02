using MedLink.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.DTOs.UserProfile
{
    public class AppointmentListItemDto
    {
        public int AppointmentId { get; set; }

        public string DoctorName { get; set; } = string.Empty;
        public string DoctorImageUrl { get; set; } = string.Empty;

        public string Specialization { get; set; } = string.Empty;

        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }

        public AppointmentStatus Status { get; set; }

        public bool CanCancel { get; set; }
        //public bool CanReschedule { get; set; }
    }
}
