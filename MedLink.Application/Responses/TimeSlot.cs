using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Responses
{
    /// <summary>
    /// فترة زمنية (مثلاً: 9:00 - 9:30)
    /// </summary>
    public class TimeSlot
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
