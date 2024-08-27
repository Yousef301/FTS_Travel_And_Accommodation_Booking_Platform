namespace TABP.Shared.Exceptions;

public class EmailSendingException : InternalServerErrorException
{
    public EmailSendingException(string message) : base(message)
    {
    }

    public EmailSendingException(string message, Exception inner) : base(message, inner)
    {
    }
}