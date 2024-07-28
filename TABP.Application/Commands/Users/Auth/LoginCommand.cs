using MediatR;

namespace TABP.Application.Commands.Users.Auth;

public class LoginCommand : IRequest<LoginResponse>
{
    public string Username { get; init; }
    public string Password { get; init; }
}