namespace TABP.Shared.Exceptions;

public class EmailTemplateException : InternalServerErrorException
{
    public EmailTemplateException(string message) : base(message)
    {
    }

    public EmailTemplateException(string message, Exception inner) : base(message, inner)
    {
    }
}