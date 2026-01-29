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
                => await _context.DoctorAvailabilities.FindAsync(id);

        public async Task<List<DoctorAvailability>> GetAvailableByDoctorAndDateAsync(int doctorId, DateTime date)
            => await _context.DoctorAvailabilities
                .Where(d => d.DoctorId == doctorId && d.Date.Date == date.Date && !d.IsBooked)
                .ToListAsync();

        public async Task MarkAsBookedAsync(int availabilityId)
        {
            var slot = await _context.DoctorAvailabilities.FindAsync(availabilityId);
            if (slot != null)
            {
                slot.IsBooked = true;
                _context.DoctorAvailabilities.Update(slot);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAsAvailableAsync(int availabilityId)
        {
            var slot = await _context.DoctorAvailabilities.FindAsync(availabilityId);
            if (slot != null)
            {
                slot.IsBooked = false;
                _context.DoctorAvailabilities.Update(slot);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(DoctorAvailability availability)
        {
            _context.DoctorAvailabilities.Update(availability);
            await _context.SaveChangesAsync();
        }
    }
}
