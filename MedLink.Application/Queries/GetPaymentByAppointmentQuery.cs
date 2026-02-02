using MediatR;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Queries
{

    public class GetPaymentByAppointmentQuery : IRequest<PaymentDto>
    {
        public int AppointmentId { get; set; }

        public GetPaymentByAppointmentQuery(int appointmentId)
        {
            AppointmentId = appointmentId;
        }
    }
}
