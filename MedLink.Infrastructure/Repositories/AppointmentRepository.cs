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
            => await _context.Appointments.Include(a => a.Schedule).FirstOrDefaultAsync(a => a.Id == id);

        public async Task<List<Appointment>> GetAppointmentsByDoctorAsync(int doctorId, DateTime? date = null)
            => await _context.Appointments
                .Where(a => a.DoctorId == doctorId && (date == null || a.Schedule.Date == date.Value.Date))
                .Include(a => a.Schedule)
                .ToListAsync();

        public async Task<List<Appointment>> GetAppointmentsByUserAsync(string userId)
            => await _context.Appointments
                .Where(a => a.BookedByUserId == userId)
                .Include(a => a.Schedule)
                .ToListAsync();

        public async Task AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task CancelAsync(Appointment appointment, string reason)
        {
            appointment.Status =AppointmentStatus.Cancelled;
            appointment.CancelledReason = reason;
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task ConfirmAsync(Appointment appointment)
        {
            appointment.Status =AppointmentStatus.Confirmed;
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }
    }
}
