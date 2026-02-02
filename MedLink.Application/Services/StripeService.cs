using MedLink.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Services
{

    public class StripeService : IStripeService
    {
        private readonly string _secretKey;

        public StripeService(IConfiguration configuration)
        {
            _secretKey = configuration["Stripe:SecretKey"]
                ?? throw new ArgumentNullException("Stripe:SecretKey is not configured");

            StripeConfiguration.ApiKey = _secretKey;
        }

        public async Task<(string PaymentIntentId, string ClientSecret)> CreatePaymentIntentAsync(
            decimal amount,
            string currency,
            string customerEmail,
            Dictionary<string, string>? metadata = null)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100), // تحويل إلى أصغر وحدة (قرش)
                Currency = currency.ToLower(),
                ReceiptEmail = customerEmail,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
                Metadata = metadata ?? new Dictionary<string, string>()
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            return (paymentIntent.Id, paymentIntent.ClientSecret);
        }

        public async Task<bool> ConfirmPaymentAsync(string paymentIntentId)
        {
            try
            {
                var service = new PaymentIntentService();
                var paymentIntent = await service.GetAsync(paymentIntentId);

                return paymentIntent.Status == "succeeded";
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CancelPaymentIntentAsync(string paymentIntentId)
        {
            try
            {
                var service = new PaymentIntentService();
                await service.CancelAsync(paymentIntentId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RefundPaymentAsync(string paymentIntentId, decimal? amount = null, string? reason = null)
        {
            try
            {
                var options = new RefundCreateOptions
                {
                    PaymentIntent = paymentIntentId,
                    Reason = reason switch
                    {
                        _ when reason?.Contains("duplicate") == true => "duplicate",
                        _ when reason?.Contains("fraud") == true => "fraudulent",
                        _ => "requested_by_customer"
                    }
                };

                if (amount.HasValue)
                {
                    options.Amount = (long)(amount.Value * 100);
                }

                var service = new RefundService();
                var refund = await service.CreateAsync(options);

                return refund.Status == "succeeded" || refund.Status == "pending";
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> GetPaymentStatusAsync(string paymentIntentId)
        {
            try
            {
                var service = new PaymentIntentService();
                var paymentIntent = await service.GetAsync(paymentIntentId);
                return paymentIntent.Status;
            }
            catch
            {
                return "unknown";
            }
        }
    }

}
