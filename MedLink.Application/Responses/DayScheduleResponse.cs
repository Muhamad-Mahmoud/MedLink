using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Responses
{
    public class DayScheduleResponse
    {
        public bool Success { get; set; }
        public DateTime Date { get; set; }
        public int TotalSlotsCreated { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<DoctorAvailabilityDto> Slots { get; set; } = new();
    }
}
