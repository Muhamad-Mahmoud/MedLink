using MedLink.Domain.Entities.Payments;
using MedLink.Domain.Enums;
using MedLink.Infrastructure.Persistence.Context;
using MedLink_Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;

    public PaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Payment?> GetByAppointmentIdAsync(int appointmentId)
           => await _context.Payments.FirstOrDefaultAsync(p => p.AppointmentId == appointmentId);

    public async Task<List<Payment>> GetPaymentsByStatusAsync(PaymentStatus status)
        => await _context.Payments.Where(p => p.Status == status).ToListAsync();

    public async Task AddAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Payment payment)
    {
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
    }

    public async Task RefundAsync(Payment payment)
    {
        payment.Status = PaymentStatus.Refunded;
        _context.Payments.Update(payment);
        await _context.SaveChangesAsync();
     
    }
}
