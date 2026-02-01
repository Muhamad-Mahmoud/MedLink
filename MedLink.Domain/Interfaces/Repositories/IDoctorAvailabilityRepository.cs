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
        Task<List<DoctorAvailability>> GetAvailableSlotsByDoctorAndDateAsync(int doctorId, DateTime date);
        Task<List<DoctorAvailability>> GetSlotsByDoctorAsync(int doctorId, DateTime? fromDate = null, DateTime? toDate = null);
        Task<DoctorAvailability> AddAsync(DoctorAvailability availability);
        Task<List<DoctorAvailability>> AddRangeAsync(List<DoctorAvailability> availabilities);
        Task<DoctorAvailability> UpdateAsync(DoctorAvailability availability);
        Task DeleteAsync(DoctorAvailability availability);
        Task<bool> IsSlotAvailableAsync(int scheduleId);
    }
}

