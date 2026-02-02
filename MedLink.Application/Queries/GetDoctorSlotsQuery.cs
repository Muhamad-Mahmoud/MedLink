using MediatR;
using MedLink_Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Queries
{

    /// <summary>
    /// جلب جميع مواعيد طبيب (متاح ومحجوز) للـ Admin
    /// </summary>
    public class GetDoctorSlotsQuery : IRequest<List<DoctorAvailabilityDto>>
    {
        public int DoctorId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public GetDoctorSlotsQuery(int doctorId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            DoctorId = doctorId;
            FromDate = fromDate;
            ToDate = toDate;
        }
    }

}
