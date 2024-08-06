using MediatR;

namespace TABP.Application.Queries.Payments.GetPayments;

public class GetPaymentsQuery : IRequest<IEnumerable<PaymentResponse>>
{
    public Guid UserId { get; set; }
}