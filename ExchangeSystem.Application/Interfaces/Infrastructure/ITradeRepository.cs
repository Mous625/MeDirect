using ExchangeSystem.Domain.Entities;

namespace ExchangeSystem.Application.Interfaces.Infrastructure;

public interface ITradeRepository
{
    Task<Trade?> GetAsync(Guid tradeId);
    Task<List<Trade>> GetAllAsync();
    Task AddAsync(Trade trade);
}