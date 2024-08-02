using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TABP.DAL.Interfaces;
using TABP.DAL.Interfaces.Repositories;

namespace TABP.Application.Services.Implementations;

public class SpecialOfferExpirationService : BackgroundService
{
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1);
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SpecialOfferExpirationService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await UpdateSpecialOffersAsync(stoppingToken);
            await Task.Delay(_checkInterval, stoppingToken);
        }
    }

    private async Task UpdateSpecialOffersAsync(CancellationToken stoppingToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var specialOfferRepository = scope.ServiceProvider.GetRequiredService<ISpecialOfferRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var expiredOffers = await specialOfferRepository.GetExpiredOffersAsync();

            foreach (var offer in expiredOffers)
            {
                offer.IsActive = false;
            }

            await unitOfWork.SaveChangesAsync();
        }
    }
}