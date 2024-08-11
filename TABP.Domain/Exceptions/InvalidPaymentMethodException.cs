namespace TABP.Domain.Exceptions;

public class InvalidPaymentMethodException : BadRequestException
{
    public InvalidPaymentMethodException()
        : base("Payment method is not valid.")
    {
    }
}