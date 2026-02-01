using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Commands
{
    public class CompleteAppointmentCommand : IRequest<Unit>
    {
        public int AppointmentId { get; set; }
        public string CompletedByUserId { get; set; } = string.Empty;  // Doctor ID
    }
}
