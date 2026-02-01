using MediatR;
using MedLink.Domain.Entities.Medical;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MedLink_Application.Queries
{
    public class GetAvailableSlotsQuery : IRequest<List<DoctorAvailabilityDto>>
    {
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }

        public GetAvailableSlotsQuery(int doctorId, DateTime date)
        {
            DoctorId = doctorId;
            Date = date;
        }
    }
}
