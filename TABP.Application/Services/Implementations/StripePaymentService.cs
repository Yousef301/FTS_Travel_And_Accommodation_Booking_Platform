using Stripe.Checkout;
using TABP.Application.Services.Interfaces;

namespace TABP.Application.Services.Implementations;

public class StripePaymentService : IPaymentService
{
    public async Task<string> CreateCheckoutSessionAsync(decimal amount, string currency, string successUrl,
        string cancelUrl)
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
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);
        return session.Url;
    }
}