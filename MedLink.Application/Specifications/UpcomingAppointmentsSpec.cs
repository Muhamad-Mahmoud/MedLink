using MedLink.Domain.Entities.Appointments;
using MedLink.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Specifications
{
    public class UpcomingAppointmentsSpec : BaseSpecification<Appointment>
    {
        public UpcomingAppointmentsSpec(string userId)
            : base(a =>
                a.UserId == userId &&
                a.Status == AppointmentStatus.Pending)
        {
        }
    }

}
