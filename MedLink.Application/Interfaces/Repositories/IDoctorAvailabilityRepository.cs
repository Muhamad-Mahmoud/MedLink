using MedLink.Domain.Entities.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Interfaces.Repositories
{
    public interface IDoctorAvailabilityRepository
    {
        Task<DoctorAvailability?> GetByIdAsync(int id);
        Task<List<DoctorAvailability>> GetAvailableByDoctorAndDateAsync(
            int doctorId,
            DateTime date
        );
        Task UpdateAsync(DoctorAvailability availability);
    }
}

