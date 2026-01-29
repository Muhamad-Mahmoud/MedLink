using MediatR;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Queries
{
    public class GetAppointmentsByDoctorQuery : IRequest<List<AppointmentDto>>
    {
        public int DoctorId { get; set; }
        public DateTime? Date { get; set; }

        public GetAppointmentsByDoctorQuery(int doctorId, DateTime? date = null)
        {
            DoctorId = doctorId;
            Date = date;
        }
    }
}
