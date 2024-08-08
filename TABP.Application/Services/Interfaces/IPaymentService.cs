namespace TABP.Application.Services.Interfaces;

public interface IPaymentService
{
    Task<string> CreateCheckoutSessionAsync(decimal amount, string currency, string successUrl, string cancelUrl);
}