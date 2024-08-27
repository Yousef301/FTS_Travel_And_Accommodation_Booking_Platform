namespace TABP.Shared.Exceptions;

public class UniqueConstraintViolationException : ConflictException
{
    public UniqueConstraintViolationException(string message) : base(message)
    {
    }
}