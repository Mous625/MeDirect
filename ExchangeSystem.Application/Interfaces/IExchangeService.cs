using ExchangeSystem.Application.Models;

namespace ExchangeSystem.Application.Interfaces;

public interface IExchangeService
{
    Task ExecuteTrade(ExecuteTradeRequest request);
}