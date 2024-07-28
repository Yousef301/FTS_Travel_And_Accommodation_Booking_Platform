using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TABP.Application.Services.Interfaces;
using TABP.DAL.Entities;

namespace TABP.Application.Services.Implementations;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user, string username)
    {
        var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(_configuration["SecretKey"]));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenClaims = new List<Claim>()
        {
            new("id", user.Id.ToString()),
            new("username", username),
            new("firstName", user.FirstName),
            new("lastName", user.LastName),
            new("email", user.Email),
            new("Role", user.Role.ToString())
        };


        var jwtToken = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            claims: tokenClaims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: signingCredentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return token;
    }
}