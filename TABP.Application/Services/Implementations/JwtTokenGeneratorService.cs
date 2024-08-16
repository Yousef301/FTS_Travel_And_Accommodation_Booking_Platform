using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;
using TABP.Domain.Services.Interfaces;

namespace TABP.Application.Services.Implementations;

public class JwtTokenGeneratorService : ITokenGeneratorService
{
    private readonly ISecretsManagerService _secretsManagerService;

    public JwtTokenGeneratorService(ISecretsManagerService secretsManagerService)
    {
        _secretsManagerService = secretsManagerService;
    }

    public string GenerateToken(User user,
        string username)
    {
        var secrets = _secretsManagerService.GetSecretAsDictionaryAsync("dev_fts_jwt").Result
                      ?? throw new ArgumentNullException(nameof(_secretsManagerService));

        var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(secrets["SecretKey"]));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenClaims = new List<Claim>()
        {
            new("id", user.Id.ToString()),
            new("username", username),
            new("firstName", user.FirstName),
            new("lastName", user.LastName),
            new("email", user.Email),
            new("role", user.Role.ToString())
        };


        var jwtToken = new JwtSecurityToken(
            issuer: secrets["Issuer"],
            audience: secrets["Audience"],
            claims: tokenClaims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: signingCredentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return token;
    }
}