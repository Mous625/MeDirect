using ExchangeSystem.Application.Models;

namespace ExchangeSystem.Application.Interfaces;

public interface ITradeService
{
    Task ExecuteTrade(ExecuteTradeRequest request);
}