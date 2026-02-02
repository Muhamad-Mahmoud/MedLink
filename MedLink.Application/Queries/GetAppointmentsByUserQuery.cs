using MediatR;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Queries
{
    public class GetAppointmentsByUserQuery : IRequest<List<AppointmentDto>>
    {
        public string UserId { get; set; }

        public GetAppointmentsByUserQuery(string userId)
        {
            UserId = userId;
        }
    }
}
