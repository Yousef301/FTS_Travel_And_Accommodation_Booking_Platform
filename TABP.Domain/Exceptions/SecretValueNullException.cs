namespace TABP.Domain.Exceptions;

public class SecretValueNullException : NotFoundException
{
    public SecretValueNullException(string message) : base(message)
    {
    }

    public SecretValueNullException(string message, Exception innerException) : base(message, innerException)
    {
    }
}