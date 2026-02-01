using MediatR;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Commands
{
    public class UpdateAppointmentCommand : IRequest<AppointmentDto>
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string PatientPhone { get; set; } = string.Empty;
        public string? PatientEmail { get; set; }
        public string? Notes { get; set; }
        public string UpdatedByUserId { get; set; } = string.Empty;
    }
}
