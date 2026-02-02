using MediatR;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Commands
{
    public class CreatePaymentCommand : IRequest<PaymentDto>
    {
        public int AppointmentId { get; set; }
        public string PaymentMethod { get; set; } = "Stripe";  // Stripe, Cash, Wallet
        public string? CustomerEmail { get; set; }
    }
}
