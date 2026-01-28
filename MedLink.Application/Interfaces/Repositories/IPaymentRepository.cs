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
        Task<Payment?> GetByAppointmentIdAsync(int appointmentId);
        Task AddAsync(Payment payment);
        Task UpdateAsync(Payment payment);
    }
}
