using ExchangeSystem.Application.Models;

namespace ExchangeSystem.Application.Interfaces;

public interface ITradeService
{
    Task<OperationResult<string?>> ExecuteTradeAsync(TradeDto dto);
    Task<OperationResult<TradeDto?>> GetTrade(Guid tradeId);
    Task<OperationResult<List<TradeDto>>> GetTradesForClient(string clientId);
}