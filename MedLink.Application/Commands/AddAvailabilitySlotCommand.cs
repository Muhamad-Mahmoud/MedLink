using MediatR;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Commands
{
    /// <summary>
    /// Add one Slot of Availability for a Doctor
    /// </summary>
    public class AddAvailabilitySlotCommand : IRequest<DoctorAvailabilityDto>
    {
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string CreatedByUserId { get; set; } = string.Empty;
    }
}
