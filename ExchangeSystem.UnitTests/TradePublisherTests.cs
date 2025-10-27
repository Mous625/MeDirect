using ExchangeSystem.Application.Exceptions;
using ExchangeSystem.Domain.Entities;
using ExchangeSystem.Infrastructure.RabbitMq;
using ExchangeSystem.Infrastructure.RabbitMq.Interfaces;
using ExchangeSystem.Infrastructure.RabbitMq.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace ExchangeSystem.UnitTests;

public class TradePublisherTests
{
    private RabbitMqPublisher _rabbitMqPublisher;
    private IChannelProvider _channelProvider;
    private ILogger<RabbitMqPublisher> _logger;
    private IOptions<RabbitMqOptions> _options;
    
    [SetUp]
    public void Setup()
    {
        _channelProvider = Substitute.For<IChannelProvider>();
        _logger = Substitute.For<ILogger<RabbitMqPublisher>>();
        _options = Options.Create(new RabbitMqOptions
        {
            HostName = "localhost",
            ExchangeName = "exchange",
            QueueName = "queue",
            ExchangeType = "direct",
            RoutingKey = ""
        });
        
        _rabbitMqPublisher = new RabbitMqPublisher(_channelProvider, _options, _logger);
    }

    [Test]
    public void Publish_Message_ThrowsException_ReturnsCustomException()
    {
        _channelProvider.GetChannel().Throws(new Exception());
        
        Assert.ThrowsAsync<PublishTradeFailedException>(async () => await _rabbitMqPublisher.PublishTradeAsync(new Trade
        {
            ClientId = null,
            Symbol = null,
            Quantity = 0,
            Price = 0
        }));
    }
}