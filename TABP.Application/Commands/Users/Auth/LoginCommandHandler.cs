using System.Security.Authentication;
using MediatR;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Interfaces.Repositories;
using TABP.Domain.Exceptions;

namespace TABP.Application.Commands.Users.Auth;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly ICredentialRepository _credentialRepository;
    private readonly ITokenGeneratorService _tokenGeneratorService;
    private readonly IPasswordService _passwordService;

    public LoginCommandHandler(ITokenGeneratorService tokenGeneratorService,
        ICredentialRepository credentialRepository, IPasswordService passwordService)
    {
        _tokenGeneratorService = tokenGeneratorService;
        _credentialRepository = credentialRepository;
        _passwordService = passwordService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _credentialRepository.GetByUsername(request.Username) ??
                   throw new NotFoundException($"User with username {request.Username} wasn't found.");

        if (!_passwordService.ValidatePassword(request.Password, user.HashedPassword))
            throw new InvalidCredentialException("Invalid username or password.");

        return new LoginResponse
        {
            Token = _tokenGeneratorService.GenerateToken(user.User, request.Username)
        };
    }
}