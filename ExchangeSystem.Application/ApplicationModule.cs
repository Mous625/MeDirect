using ExchangeSystem.Application.Interfaces;
using ExchangeSystem.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeSystem.Application;

public static class ApplicationModule
{
    public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<ITradeService, TradeService>();
    }     
}