using MediatR;

namespace TABP.Application.Commands.Payments.PaymentWebhook;

public class PaymentWebhookCommand : IRequest
{
    public string DataStream { get; set; }
    public string Signature { get; set; }
}