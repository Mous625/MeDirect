using ExchangeSystem.Domain.Entities;

namespace ExchangeSystem.Domain.Abstractions;

public interface ITradeRepository
{
    Task AddAsync(Trade trade);
}