using MedLink.Domain.Entities.Appointments;
using MedLink.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MedLink_Application.Specifications
{
    public class UpcomingAppointmentsByUserSpec
     : BaseSpecification<Appointment>
    {
        public UpcomingAppointmentsByUserSpec(string userId)
            : base(a =>
                a.UserId == userId &&
                a.Status != AppointmentStatus.Cancelled &&
                (
                    a.Schedule.Date > DateTime.UtcNow.Date ||
                    (
                        a.Schedule.Date == DateTime.UtcNow.Date &&
                        a.Schedule.StartTime >= DateTime.UtcNow.TimeOfDay
                    )
                )
            )
        {
            AddIncludes(q =>
                q.Include(a => a.Doctor)
                 .ThenInclude(d => d.Specialization)
            );

            AddIncludes(q => q.Include(a => a.Schedule));

            AddOrderBy(a => a.Schedule.Date);
        }
    }




}
