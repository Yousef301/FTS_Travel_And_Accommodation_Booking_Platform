using TABP.DAL.Entities;

namespace TABP.Application.Services.Interfaces;

public interface ITokenGeneratorService
{
    public string GenerateToken(User user, string username);
}