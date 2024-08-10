using Stripe;
using Stripe.Checkout;
using TABP.Application.Services.Interfaces;

namespace TABP.Application.Services.Implementations;

public class StripePaymentService : IPaymentService
{
    public async Task<string> CreateCheckoutSessionAsync(decimal amount, string currency, string successUrl,
        string cancelUrl, string bookingId, string userId, string userEmail)
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
                            UnitAmount = (long)(amount * 100),
                            Currency = currency,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Product Name",
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                Metadata = new Dictionary<string, string>
                {
                    { "booking_id", bookingId },
                    { "user_id", userId },
                    { "user_email", userEmail }
                }
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);
            return session.Url;
        }
        catch (StripeException ex)
        {
            throw new ApplicationException("Error creating Stripe checkout session", ex);
        }
    }
}