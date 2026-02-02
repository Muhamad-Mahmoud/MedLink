using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Responses
{
    public class AddSlotRequest
    {
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; } = string.Empty; // "09:00"
        public string EndTime { get; set; } = string.Empty;   // "09:30"
    }
}
