using ExchangeSystem.Application.Interfaces.InfrastructureInterfaces;
using ExchangeSystem.Application.Models;

namespace ExchangeSystem.Infrastructure.Database;

internal class DatabaseService : IExchangeService
{
    public async Task ExecuteTrade(ExecuteTradeRequest request)
    {
        return;
    }
}