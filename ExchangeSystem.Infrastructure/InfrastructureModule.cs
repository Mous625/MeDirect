using ExchangeSystem.Application.Interfaces.InfrastructureInterfaces;
using ExchangeSystem.Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeSystem.Infrastructure;

public static class InfrastructureModule
{
    public static void ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IExchangeService, DatabaseService>();
    }     
}