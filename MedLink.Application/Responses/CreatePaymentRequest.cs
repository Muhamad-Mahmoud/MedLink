using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Responses
{
    public class CreatePaymentRequest
    {
        public int AppointmentId { get; set; }
        public string PaymentMethod { get; set; } = "Stripe";
        public string? CustomerEmail { get; set; }
    }
}
