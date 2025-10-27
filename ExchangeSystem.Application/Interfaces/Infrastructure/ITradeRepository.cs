using ExchangeSystem.Domain.Entities;

namespace ExchangeSystem.Application.Interfaces.Infrastructure;

public interface ITradeRepository
{
    Task<Trade?> GetAsync(Guid tradeId);
    Task<List<Trade>> GetAllByClientIdAsync(string id);
    Task AddAsync(Trade trade);
}