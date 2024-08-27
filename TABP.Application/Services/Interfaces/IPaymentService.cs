
using TABP.Shared.Models;

namespace TABP.Application.Services.Interfaces;

public interface IPaymentService
{
    Task<string> CreateCheckoutSessionAsync(PaymentData paymentData);
}