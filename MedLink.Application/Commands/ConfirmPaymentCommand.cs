using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Commands
{
    public class ConfirmPaymentCommand : IRequest<Unit>
    {
        public string PaymentIntentId { get; set; } = string.Empty;
    }
}
