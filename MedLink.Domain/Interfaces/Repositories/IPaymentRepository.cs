using MedLink.Domain.Entities.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(int id);
        Task<Payment?> GetByAppointmentIdAsync(int appointmentId);
        Task<Payment?> GetByStripePaymentIntentIdAsync(string paymentIntentId);
        Task<List<Payment>> GetPaymentsByUserAsync(string userId);
        Task<Payment> AddAsync(Payment payment);
        Task<Payment> UpdateAsync(Payment payment);
        Task<bool> ExistsForAppointmentAsync(int appointmentId);
    }
}
