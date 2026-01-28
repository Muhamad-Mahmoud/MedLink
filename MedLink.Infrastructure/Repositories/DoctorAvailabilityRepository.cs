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
            return await _context.DoctorAvailabilities.FindAsync(id);
        }

        public async Task<List<DoctorAvailability>> GetAvailableByDoctorAndDateAsync(
            int doctorId,
            DateTime date)
        {
            return await _context.DoctorAvailabilities
                .Where(x =>
                    x.DoctorId == doctorId &&
                    x.Date.Date == date.Date &&
                    !x.IsBooked)
                .ToListAsync();
        }

        public async Task UpdateAsync(DoctorAvailability availability)
        {
            _context.DoctorAvailabilities.Update(availability);
            await _context.SaveChangesAsync();
        }
    }
}
