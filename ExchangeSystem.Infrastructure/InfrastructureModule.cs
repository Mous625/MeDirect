using ExchangeSystem.Domain.Abstractions;
using ExchangeSystem.Infrastructure.Database;
using ExchangeSystem.Infrastructure.Rabbit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeSystem.Infrastructure;

public static class InfrastructureModule
{
    public static void ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<TradeContext>(options => 
            options.UseMySql(config["Database:ConnectionString"], ServerVersion.AutoDetect(config["Database:ConnectionString"])));
        
        services.AddScoped<ITradeRepository, TradeContext>();

        services.AddSingleton<IQueueServiceHandler, RabbitMqBase>();
    }     
}