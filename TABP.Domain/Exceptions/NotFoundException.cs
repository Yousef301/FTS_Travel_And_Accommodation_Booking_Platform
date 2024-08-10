namespace TABP.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
    
    // throw new NotFoundException($"Entity with id {id} was not found.");
    
    // Status404NotFound
}