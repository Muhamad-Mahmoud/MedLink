

namespace MedLink.Domain.Interfaces.Repositories
{
    public interface IStripeService
    {

        Task<(string PaymentIntentId, string ClientSecret)> CreatePaymentIntentAsync(
            decimal amount,
            string currency,
            string customerEmail,
            Dictionary<string, string>? metadata = null);

       
        Task<bool> ConfirmPaymentAsync(string paymentIntentId);

    
        Task<bool> CancelPaymentIntentAsync(string paymentIntentId);

  
        Task<bool> RefundPaymentAsync(string paymentIntentId, decimal? amount = null, string? reason = null);

        Task<string> GetPaymentStatusAsync(string paymentIntentId);
    }
}
