using ExchangeSystem.Application.Interfaces;
using ExchangeSystem.Application.Interfaces.Infrastructure;
using ExchangeSystem.Infrastructure.Database;
using ExchangeSystem.Infrastructure.RabbitMq;
using ExchangeSystem.Infrastructure.RabbitMq.Interfaces;
using ExchangeSystem.Infrastructure.RabbitMq.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeSystem.Infrastructure;

public static class InfrastructureModule
{
    public static void ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        var databaseConnectionString = config["Database:ConnectionString"] ??
                                       throw new InvalidOperationException("Database:ConnectionString");

        services.AddDbContext<TradeContext>(options =>
            options.UseMySql(databaseConnectionString, ServerVersion.AutoDetect(databaseConnectionString)));

        services.AddScoped<ITradeRepository, TradeContext>();

        services.AddOptions<RabbitMqOptions>()
            .Bind(config.GetSection("RabbitMq"))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.AddSingleton<IStartupTask, RabbitMqTopologyGenerator>();

        services.AddSingleton<IChannelProvider, RabbitMqBase>();

        services.AddSingleton<IQueuePublisher, RabbitMqPublisher>();
    }
}