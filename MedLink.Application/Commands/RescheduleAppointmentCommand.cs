using MediatR;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Commands
{
    public class RescheduleAppointmentCommand : IRequest<AppointmentDto>
    {
        public int AppointmentId { get; set; }
        public int NewScheduleId { get; set; }
        public string RescheduledByUserId { get; set; } = string.Empty;
    }
}
