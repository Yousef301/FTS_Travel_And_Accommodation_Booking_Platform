namespace TABP.Domain.Exceptions;

public class AlreadyExistsException : ConflictException
{
    public AlreadyExistsException(string message) : base(message)
    {
    }
}