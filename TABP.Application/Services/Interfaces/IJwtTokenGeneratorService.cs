using TABP.DAL.Entities;

namespace TABP.Application.Services.Interfaces;

public interface IJwtTokenGeneratorService
{
    public string GenerateToken(User user, string username);
}