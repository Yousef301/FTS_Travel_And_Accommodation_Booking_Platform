namespace TABP.Shared.Exceptions;

public class AlreadyExistsException : ConflictException
{
    public AlreadyExistsException(string message) : base(message)
    {
    }
}