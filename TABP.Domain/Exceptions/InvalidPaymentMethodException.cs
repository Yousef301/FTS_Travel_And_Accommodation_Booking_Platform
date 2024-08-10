namespace TABP.Domain.Exceptions;

public class InvalidPaymentMethodException : Exception
{
    public InvalidPaymentMethodException()
        : base("Payment method is not valid.")
    {
    }
}