namespace TABP.Domain.Exceptions;

public class StripePaymentException : Exception
{
    public StripePaymentException(string message) : base(message)
    {
    }

    public StripePaymentException(string message, Exception innerException) : base(message, innerException)
    {
    }
}