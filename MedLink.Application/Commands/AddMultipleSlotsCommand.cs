using MediatR;
using MedLink.Application.Responses;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Commands
{
    /// <summary>
    /// أمر إضافة مواعيد متعددة للطبيب (Bulk)
    /// مثال: إضافة كل مواعيد الأسبوع مرة واحدة
    /// </summary>
    public class AddMultipleSlotsCommand : IRequest<List<DoctorAvailabilityDto>>
    {
        public int DoctorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<DayOfWeek> WorkingDays { get; set; } = new();
        public List<TimeSlot> TimeSlots { get; set; } = new();
        public string CreatedByUserId { get; set; } = string.Empty;
    }

}
