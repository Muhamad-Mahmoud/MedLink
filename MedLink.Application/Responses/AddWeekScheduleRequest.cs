using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Responses
{
    public class AddWeekScheduleRequest
    {
        public int DoctorId { get; set; }
        public DateTime StartDate { get; set; }
    }

}
