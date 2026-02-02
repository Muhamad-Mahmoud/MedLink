using MediatR;
using MedLink.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Commands
{
    /// <summary>
    /// أمر إضافة مواعيد ليوم محدد بفترة زمنية معينة
    /// مثال: يوم 2-2-2026 من 10ص لـ 5م، كل موعد 15 دقيقة
    /// 
    /// </summary>
    public class AddDayScheduleCommand : IRequest<DayScheduleResponse>
    {
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; } = string.Empty;  // "10:00"
        public string EndTime { get; set; } = string.Empty;    // "17:00" (5 PM)
        public int SlotDurationMinutes { get; set; } = 15;     // Default: 15 minutes
        public string CreatedByUserId { get; set; } = string.Empty;
    }

}
