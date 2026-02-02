using MediatR;
using MedLink_Application.Commands;
using MedLink_Application.Queries;
using MedLink_Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;

namespace Medical_Team_B.Controllers
{
    // [Authorize]

    public class PaymentsController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public PaymentsController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        
        // 1. Create Payment
        // POST: api/payments
      
        [HttpPost]
        public async Task<ActionResult<PaymentDto>> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            try
            {
                var command = new CreatePaymentCommand
                {
                    AppointmentId = request.AppointmentId,
                    PaymentMethod = request.PaymentMethod,
                    CustomerEmail = request.CustomerEmail
                };

                var payment = await _mediator.Send(command);

                return Ok(new
                {
                    paymentId = payment.Id,
                    clientSecret = payment.StripeClientSecret,
                    amount = payment.Amount,
                    currency = payment.Currency,
                    status = payment.Status
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

      
        /// 2. Confirm Payment
        /// POST: api/payments/confirm
       
        [HttpPost("confirm")]
        public async Task<ActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequest request)
        {
            try
            {
                var command = new ConfirmPaymentCommand
                {
                    PaymentIntentId = request.PaymentIntentId
                };

                await _mediator.Send(command);
                return Ok(new { message = "Payment confirmed successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

       
        /// 3. Get Payment By Appointment 
        /// GET: api/payments/appointment/{appointmentId}
    
        [HttpGet("appointment/{appointmentId}")]
        public async Task<ActionResult<PaymentDto>> GetPaymentByAppointment(int appointmentId)
        {
            try
            {
                var query = new GetPaymentByAppointmentQuery(appointmentId);
                var payment = await _mediator.Send(query);
                return Ok(payment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // 4. Get My Payments 
        // GET: api/payments/my-payments
        [HttpGet("my-payments")]
        public async Task<ActionResult<List<PaymentDto>>> GetMyPayments()
        {
            var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            var query = new GetMyPaymentsQuery(userId);
            var payments = await _mediator.Send(query);
            return Ok(payments);
        }

       
        /// 5. Stripe Webhook
        /// POST: api/payments/webhook
        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeSignature = Request.Headers["Stripe-Signature"].ToString();
                var webhookSecret = _configuration["Stripe:WebhookSecret"];

                Event stripeEvent;

                if (!string.IsNullOrEmpty(webhookSecret))
                {
                    try
                    {
                        stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, webhookSecret);
                    }
                    catch (StripeException e)
                    {
                        Console.WriteLine($"Webhook signature verification failed: {e.Message}");
                        return BadRequest();
                    }
                }
                else
                {
                    stripeEvent = EventUtility.ParseEvent(json);
                }

                if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    if (paymentIntent != null)
                    {
                        Console.WriteLine($"Payment succeeded: {paymentIntent.Id}");

                        var command = new ConfirmPaymentCommand
                        {
                            PaymentIntentId = paymentIntent.Id
                        };

                        await _mediator.Send(command);
                    }
                }
                else if (stripeEvent.Type == "payment_intent.payment_failed")
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    if (paymentIntent != null)
                    {
                        Console.WriteLine($"Payment failed: {paymentIntent.Id}");
                    }
                }
                else if (stripeEvent.Type == "payment_intent.canceled")
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    if (paymentIntent != null)
                    {
                        Console.WriteLine($"Payment canceled: {paymentIntent.Id}");
                    }
                }
                else
                {
                    Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Webhook error: {ex.Message}");
                return BadRequest();
            }
        }

    }
}
