namespace TABP.Domain.Exceptions;

public class EmailSendingException : Exception
{
    public EmailSendingException(string message) : base(message)
    {
    }

    public EmailSendingException(string message, Exception inner) : base(message, inner)
    {
    }
}