using ExchangeSystem.Application.Models;

namespace ExchangeSystem.Application.Interfaces.InfrastructureInterfaces;

public interface IExchangeService
{
    Task ExecuteTrade(ExecuteTradeRequest request);
}