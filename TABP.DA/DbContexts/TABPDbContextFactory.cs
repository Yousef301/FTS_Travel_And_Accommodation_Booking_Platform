using Amazon.SecretsManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TABP.Domain.Services.Implementations;

namespace TABP.DAL.DbContexts;

public class TABPDbContextFactory : IDesignTimeDbContextFactory<TABPDbContext>
{
    public TABPDbContext CreateDbContext(string[] args)
    {
        var secretsManagerService = new SecretsManagerService(new AmazonSecretsManagerClient());

        var secrets = secretsManagerService.GetSecretAsDictionaryAsync("dev_fts_database").Result
                      ?? throw new ArgumentNullException(nameof(secretsManagerService));

        var optionsBuilder = new DbContextOptionsBuilder<TABPDbContext>();
        var connectionString = secrets["TABPDb"];

        optionsBuilder.UseSqlServer(connectionString);

        return new TABPDbContext(optionsBuilder.Options);
    }
}