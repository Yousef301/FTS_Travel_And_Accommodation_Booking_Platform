using System.Text.Json;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using TABP.Shared.Exceptions;
using TABP.Shared.Services.Interfaces;

namespace TABP.Shared.Services.Implementations;

public class SecretsManagerService : ISecretsManagerService
{
    private readonly IAmazonSecretsManager _secretsManager;

    public SecretsManagerService(IAmazonSecretsManager secretsManager)
    {
        _secretsManager = secretsManager;
    }

    public async Task<Dictionary<string, string>?> GetSecretAsDictionaryAsync(string secretName)
    {
        var request = new GetSecretValueRequest
        {
            SecretId = secretName
        };

        var response = await _secretsManager.GetSecretValueAsync(request);

        if (response.SecretString != null)
        {
            return JsonSerializer.Deserialize<Dictionary<string, string>>(response.SecretString);
        }

        throw new SecretValueNullException($"Secret value for '{secretName}' is null.");
    }
}