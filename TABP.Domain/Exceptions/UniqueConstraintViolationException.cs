namespace TABP.Domain.Exceptions;

public class UniqueConstraintViolationException : ConflictException
{
    public UniqueConstraintViolationException(string message) : base(message)
    {
    }
}