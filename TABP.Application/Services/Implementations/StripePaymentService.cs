using Stripe;
using Stripe.Checkout;
using TABP.Application.Services.Interfaces;
using TABP.Domain.Exceptions;
using TABP.Domain.Models;

namespace TABP.Application.Services.Implementations;

public class StripePaymentService : IPaymentService
{
    public async Task<string> CreateCheckoutSessionAsync(PaymentData paymentData)
    {
        try
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(paymentData.TotalPrice * 100),
                            Currency = paymentData.Currency,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Product Name",
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = paymentData.SuccessUrl,
                CancelUrl = paymentData.CancelUrl,
                Metadata = new Dictionary<string, string>
                {
                    { "booking_id", paymentData.BookingId.ToString() },
                    { "user_id", paymentData.UserId.ToString() },
                    { "user_email", paymentData.UserEmail }
                }
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);
            return session.Url;
        }
        catch (StripeException ex)
        {
            throw new StripePaymentException("Error creating Stripe checkout session", ex);
        }
    }
}