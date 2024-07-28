namespace TABP.Application.Exceptions;

public class EmailAlreadyExist : Exception
{
    public string Title => "Email already exist";
}