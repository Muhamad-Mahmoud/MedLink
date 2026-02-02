using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Responses
{
    public class AddCustomScheduleRequest
    {
        public int DoctorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<DayOfWeek>? WorkingDays { get; set; }
        public List<TimeSlotRequest>? TimeSlots { get; set; }
    }

}
