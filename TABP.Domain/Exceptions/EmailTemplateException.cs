namespace TABP.Domain.Exceptions;

public class EmailTemplateException : Exception
{
    public EmailTemplateException(string message) : base(message)
    {
    }

    public EmailTemplateException(string message, Exception inner) : base(message, inner)
    {
    }
}