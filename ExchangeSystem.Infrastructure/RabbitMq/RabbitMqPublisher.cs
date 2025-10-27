using System.Text;
using System.Text.Json;
using ExchangeSystem.Application.Exceptions;
using ExchangeSystem.Application.Interfaces.Infrastructure;
using ExchangeSystem.Domain.Entities;
using ExchangeSystem.Infrastructure.RabbitMq.Interfaces;
using ExchangeSystem.Infrastructure.RabbitMq.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ExchangeSystem.Infrastructure.RabbitMq;

internal class RabbitMqPublisher : IQueuePublisher
{
    private readonly IChannelProvider _channelProvider;
    private readonly RabbitMqOptions _rabbitMqOptions;
    private readonly ILogger<RabbitMqPublisher> _logger;

    public RabbitMqPublisher(IChannelProvider channelProvider, IOptions<RabbitMqOptions> options,
        ILogger<RabbitMqPublisher> logger)
    {
        _channelProvider = channelProvider;
        _rabbitMqOptions = options.Value;
        _logger = logger;
    }

    public async Task PublishTradeAsync(Trade message)
    {
        try
        {
            var serialisedMessage = JsonSerializer.Serialize(message);

            var body = Encoding.UTF8.GetBytes(serialisedMessage);

            await _channelProvider.GetChannel()
                .BasicPublishAsync(
                    exchange: _rabbitMqOptions.ExchangeName,
                    routingKey: _rabbitMqOptions.RoutingKey,
                    body: body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured whilst publishing the trade.");
            throw new PublishTradeFailedException();
        }
    }
}