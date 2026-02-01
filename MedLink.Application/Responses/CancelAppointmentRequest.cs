using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Responses
{
    public class CancelAppointmentRequest
    {
        public string Reason { get; set; } = string.Empty;
    }
}
