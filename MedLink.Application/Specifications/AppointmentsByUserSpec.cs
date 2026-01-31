using MedLink.Domain.Entities.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Specifications
{
    public class AppointmentsByUserSpec : BaseSpecification<Appointment>
    {
        public AppointmentsByUserSpec(string userId)
            : base(a => a.UserId == userId)
        {
        }
    }

}
