using ExchangeSystem.Domain.Entities;

namespace ExchangeSystem.Application.Interfaces.Infrastructure;

public interface IQueuePublisher
{
    Task PublishTradeAsync(Trade message);
}