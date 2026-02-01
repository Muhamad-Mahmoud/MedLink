using MedLink.Domain.Entities.Appointments;
using MedLink.Infrastructure.Persistence.Context;
using MedLink_Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Infrastructure.Repositories
{
    public class DoctorAvailabilityRepository : IDoctorAvailabilityRepository
    {

        private readonly ApplicationDbContext _context;

        public DoctorAvailabilityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DoctorAvailability?> GetByIdAsync(int id)
        {
            return await _context.DoctorAvailabilities
                .Include(da => da.Doctor)
                .FirstOrDefaultAsync(da => da.Id == id && !da.IsDeleted);
        }

        public async Task<List<DoctorAvailability>> GetAvailableSlotsByDoctorAndDateAsync(int doctorId, DateTime date)
        {
            return await _context.DoctorAvailabilities
                .Include(da => da.Doctor)
                .Where(da => da.DoctorId == doctorId
                          && da.Date.Date == date.Date
                          && !da.IsBooked
                          && !da.IsDeleted)
                .OrderBy(da => da.StartTime)
                .ToListAsync();
        }

        public async Task<List<DoctorAvailability>> GetSlotsByDoctorAsync(int doctorId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.DoctorAvailabilities
                .Include(da => da.Doctor)
                .Where(da => da.DoctorId == doctorId && !da.IsDeleted);

            if (fromDate.HasValue)
            {
                query = query.Where(da => da.Date.Date >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                query = query.Where(da => da.Date.Date <= toDate.Value.Date);
            }

            return await query
                .OrderBy(da => da.Date)
                .ThenBy(da => da.StartTime)
                .ToListAsync();
        }

        public async Task<DoctorAvailability> AddAsync(DoctorAvailability availability)
        {
            await _context.DoctorAvailabilities.AddAsync(availability);
            await _context.SaveChangesAsync();
            return availability;
        }

        public async Task<List<DoctorAvailability>> AddRangeAsync(List<DoctorAvailability> availabilities)
        {
            await _context.DoctorAvailabilities.AddRangeAsync(availabilities);
            await _context.SaveChangesAsync();
            return availabilities;
        }

        public async Task<DoctorAvailability> UpdateAsync(DoctorAvailability availability)
        {
            availability.UpdatedAt = DateTime.UtcNow;
            _context.DoctorAvailabilities.Update(availability);
            await _context.SaveChangesAsync();
            return availability;
        }

        public async Task DeleteAsync(DoctorAvailability availability)
        {
            availability.IsDeleted = true;
            availability.UpdatedAt = DateTime.UtcNow;
            _context.DoctorAvailabilities.Update(availability);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsSlotAvailableAsync(int scheduleId)
        {
            var slot = await _context.DoctorAvailabilities
                .FirstOrDefaultAsync(da => da.Id == scheduleId && !da.IsDeleted);

            return slot != null && !slot.IsBooked;
        }
    }
}
