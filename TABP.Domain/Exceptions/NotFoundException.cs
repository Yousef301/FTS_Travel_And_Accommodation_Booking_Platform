namespace TABP.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string entity, Guid id) : base($"{entity} with ID {id} was not found.")
    {
    }

    public NotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}