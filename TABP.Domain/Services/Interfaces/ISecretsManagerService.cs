namespace TABP.Domain.Services.Interfaces;

public interface ISecretsManagerService
{
    Task<Dictionary<string, string>?> GetSecretAsDictionaryAsync(string secretName);
}