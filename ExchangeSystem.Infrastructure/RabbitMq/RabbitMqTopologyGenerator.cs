using ExchangeSystem.Application.Interfaces;
using ExchangeSystem.Application.Interfaces.Infrastructure;
using ExchangeSystem.Infrastructure.RabbitMq.Interfaces;
using ExchangeSystem.Infrastructure.RabbitMq.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ExchangeSystem.Infrastructure.RabbitMq;

internal class RabbitMqTopologyGenerator : IStartupTask
{
    private readonly IChannelProvider _channelProvider;
    private readonly RabbitMqOptions _options;

    public RabbitMqTopologyGenerator(IChannelProvider channelProvider, IOptions<RabbitMqOptions> options)
    {
        _channelProvider = channelProvider;
        _options = options.Value;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var channel = await _channelProvider.CreateAndGetChannelIfNotExistsAsync(CancellationToken.None);

        await channel.ExchangeDeclareAsync(
            exchange: _options.RoutingKey, 
            durable: true, 
            type: ExchangeType.Fanout,
            cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: _options.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false, 
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queue: _options.QueueName, 
            exchange: _options.ExchangeName,
            routingKey: _options.RoutingKey, 
            cancellationToken: cancellationToken);
    }
}