using TABP.DAL.Entities;

namespace TABP.Application.Services.Interfaces;

public interface IJwtTokenGenerator
{
    public string GenerateToken(User user, string username);
}