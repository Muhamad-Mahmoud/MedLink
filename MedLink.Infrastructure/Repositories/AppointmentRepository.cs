using MedLink.Domain.Entities.Appointments;
using MedLink.Domain.Enums;
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
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AppointmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
        }

        public async Task<Appointment?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Schedule)
                .Include(a => a.Payment)
                .Include(a => a.BookedByUser)
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);
        }

        public async Task<List<Appointment>> GetAppointmentsByDoctorAsync(int doctorId, DateTime? date = null)
        {
            var query = _context.Appointments
                .Include(a => a.Schedule)
                .Include(a => a.Payment)
                .Where(a => a.DoctorId == doctorId && !a.IsDeleted);

            if (date.HasValue)
            {
                query = query.Where(a => a.Schedule.Date.Date == date.Value.Date);
            }

            return await query.OrderBy(a => a.Schedule.Date).ThenBy(a => a.Schedule.StartTime).ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByUserAsync(string userId)
        {
            return await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Schedule)
                .Include(a => a.Payment)
                .Where(a => a.BookedByUserId == userId && !a.IsDeleted)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<Appointment> AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment> UpdateAsync(Appointment appointment)
        {
            appointment.UpdatedAt = DateTime.UtcNow;
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task DeleteAsync(Appointment appointment)
        {
            appointment.IsDeleted = true;
            appointment.UpdatedAt = DateTime.UtcNow;
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Appointments.AnyAsync(a => a.Id == id && !a.IsDeleted);
        }
    }
}
