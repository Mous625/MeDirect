using RabbitMQ.Client;

namespace ExchangeSystem.Infrastructure.RabbitMq.Interfaces;

public interface IChannelProvider
{
    Task<IChannel> CreateAndGetChannelIfNotExistsAsync(CancellationToken ct);
    IChannel GetChannel();
}