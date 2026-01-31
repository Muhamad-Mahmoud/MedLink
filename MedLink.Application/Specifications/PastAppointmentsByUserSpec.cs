using MedLink.Domain.Entities.Appointments;
using MedLink.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MedLink_Application.Specifications
{
    public class PastAppointmentsByUserSpec
        : BaseSpecification<Appointment>
    {
        public PastAppointmentsByUserSpec(string userId)
            : base(a =>
                a.UserId == userId &&
                a.Status != AppointmentStatus.Cancelled &&
                (
                    a.Schedule.Date < DateTime.UtcNow.Date ||
                    (
                        a.Schedule.Date == DateTime.UtcNow.Date &&
                        a.Schedule.StartTime < DateTime.UtcNow.TimeOfDay
                    )
                )
            )
        {
            AddIncludes(q =>
                q.Include(a => a.Doctor)
                 .ThenInclude(d => d.Specialization)
            );

            AddIncludes(q => q.Include(a => a.Schedule));

            AddOrderByDesc(a => a.Schedule.Date);
        }
    }
}
