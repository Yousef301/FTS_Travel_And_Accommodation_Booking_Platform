namespace TABP.Web.Services.Interfaces;

public interface IUserContext
{
    Guid Id { get; }
    string Role { get; }
    string Email { get; }
}