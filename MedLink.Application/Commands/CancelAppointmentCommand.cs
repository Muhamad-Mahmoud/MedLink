using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Commands
{
    public class CancelAppointmentCommand : IRequest<bool>
    {
        public int AppointmentId { get; set; }
        public string Reason { get; set; }

        public CancelAppointmentCommand(int appointmentId, string reason)
        {
            AppointmentId = appointmentId;
            Reason = reason;
        }
    }
}
