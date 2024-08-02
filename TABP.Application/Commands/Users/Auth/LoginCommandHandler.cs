using System.Security.Authentication;
using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Commands.Users.Auth;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly ICredentialRepository _credentialRepository;
    private readonly IJwtTokenGeneratorService _jwtTokenGeneratorService;
    private readonly IPasswordService _passwordService;

    public LoginCommandHandler(IJwtTokenGeneratorService jwtTokenGeneratorService,
        ICredentialRepository credentialRepository, IPasswordService passwordService)
    {
        _jwtTokenGeneratorService = jwtTokenGeneratorService;
        _credentialRepository = credentialRepository;
        _passwordService = passwordService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _credentialRepository.GetByUsername(request.Username) ??
                   throw new InvalidCredentialException();

        if (!_passwordService.ValidatePassword(request.Password, user.HashedPassword))
            throw new InvalidCredentialException();

        return new LoginResponse()
        {
            Token = _jwtTokenGeneratorService.GenerateToken(user.User, request.Username)
        };
    }
}