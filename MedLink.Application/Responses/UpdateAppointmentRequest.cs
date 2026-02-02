using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Responses
{
    public class UpdateAppointmentRequest
    {
        public string PatientName { get; set; } = string.Empty;
        public string PatientPhone { get; set; } = string.Empty;
        public string? PatientEmail { get; set; }
        public string? Notes { get; set; }
    }

}
