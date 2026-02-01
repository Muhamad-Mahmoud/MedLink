using MediatR;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Queries
{

    public class GetMyPaymentsQuery : IRequest<List<PaymentDto>>
    {
        public string UserId { get; set; }

        public GetMyPaymentsQuery(string userId)
        {
            UserId = userId;
        }
    }

}
