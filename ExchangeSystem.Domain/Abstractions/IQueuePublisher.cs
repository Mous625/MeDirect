using ExchangeSystem.Domain.Entities;

namespace ExchangeSystem.Domain.Abstractions;

public interface IQueuePublisher
{
    Task PublishTradeAsync(Trade message);
}