
using MedLink.Domain.Entities.Appointments;

namespace MedLink_Application.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> GetByIdAsync(int id);

        // Get appointments by doctor (optionally by date)
        Task<List<Appointment>> GetAppointmentsByDoctorAsync(int doctorId, DateTime? date = null);

        // Get appointments by user
        Task<List<Appointment>> GetAppointmentsByUserAsync(string userId);

        // Add new appointment
        Task AddAsync(Appointment appointment);

        // Update existing appointment
        Task UpdateAsync(Appointment appointment);

        // Cancel an appointment with reason
        Task CancelAsync(Appointment appointment, string reason);

        // Confirm an appointment
        Task ConfirmAsync(Appointment appointment);

    }
}
