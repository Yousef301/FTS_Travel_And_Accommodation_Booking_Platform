namespace TABP.Domain.Exceptions;

public class UniqueConstraintViolationException : Exception
{
    public UniqueConstraintViolationException(string message) : base(message)
    {
    }

    // throw new UniqueConstraintViolationException($"An entity with the same '{entity.UniqueField}' already exists.");
    
    // Status409Conflict
}