namespace TABP.Shared.Exceptions;

public class StripePaymentException : PaymentRequiredException
{
    public StripePaymentException(string message) : base(message)
    {
    }

    public StripePaymentException(string message, Exception innerException) : base(message, innerException)
    {
    }
}