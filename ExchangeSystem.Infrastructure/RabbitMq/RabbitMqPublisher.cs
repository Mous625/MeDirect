using System.Text;
using System.Text.Json;
using ExchangeSystem.Domain.Abstractions;
using ExchangeSystem.Domain.Entities;
using ExchangeSystem.Infrastructure.RabbitMq.Interfaces;
using ExchangeSystem.Infrastructure.RabbitMq.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ExchangeSystem.Infrastructure.RabbitMq;

internal class RabbitMqPublisher : IQueuePublisher
{
    private readonly IChannelProvider _channelProvider;
    private readonly RabbitMqOptions _rabbitMqOptions;

    public RabbitMqPublisher(IChannelProvider channelProvider, IOptions<RabbitMqOptions> options)
    {
        _channelProvider = channelProvider;
        _rabbitMqOptions = options.Value;
    }

    public async Task PublishTradeAsync(Trade message)
    {
        var serialisedMessage = JsonSerializer.Serialize(message);

        var body = Encoding.UTF8.GetBytes(serialisedMessage);

        await _channelProvider.GetChannel()
            .BasicPublishAsync(
                exchange: _rabbitMqOptions.ExchangeName, 
                routingKey: _rabbitMqOptions.RoutingKey,
                body: body);
    }
}