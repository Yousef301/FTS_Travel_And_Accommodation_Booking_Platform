namespace TABP.Domain.Exceptions;

public class PaymentRequiredException : Exception
{
    public PaymentRequiredException(string message) : base(message)
    {
    }

    public PaymentRequiredException(string message, Exception innerException) : base(message, innerException)
    {
    }
}