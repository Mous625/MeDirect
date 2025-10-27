using ExchangeSystem.Application.Interfaces.Infrastructure;
using ExchangeSystem.Application.Models;
using ExchangeSystem.Application.Services;
using ExchangeSystem.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace ExchangeSystem.UnitTests;

public class TradeServiceTests
{
    private ITradeRepository _repo;
    private IQueuePublisher _publisher;
    private ILogger<TradeService> _logger;
    private TradeService _tradeService;

    [SetUp]
    public void Setup()
    {
        _repo = Substitute.For<ITradeRepository>();
        _publisher = Substitute.For<IQueuePublisher>();
        _logger = Substitute.For<ILogger<TradeService>>();

        _tradeService = new TradeService(_repo, _publisher, _logger);
    }

    [Test]
    public async Task ExecuteTradeAsync_WhenCalled_SavesTrade_And_Publishes_And_Returns_Id()
    {
        // Arrange
        var tradeDto = CreateSampleTradeDto();

        // Act
        var result = await _tradeService.ExecuteTradeAsync(tradeDto);

        // Assert
        await _repo.Received(1).AddAsync(Arg.Is<Trade>(t =>
            t.ClientId == tradeDto.ClientId &&
            t.Symbol == tradeDto.Symbol &&
            t.Quantity == tradeDto.Quantity &&
            t.Price == tradeDto.Price));

        await _publisher.Received(1).PublishTradeAsync(Arg.Any<Trade>());

        result.Should().NotBeNull();
        result.Result.Should().NotBeNull();
        result.Result.Should().Be(Guid.Empty.ToString());
    }

    [Test]
    public async Task GetTrade_WhenTradeMissing_ReturnsNull()
    {
        // Arrange
        _repo.GetAsync(Arg.Any<Guid>()).Returns((Trade?)null);

        // Act
        var result = await _tradeService.GetTrade(Guid.NewGuid());

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeNull();
    }    

    [Test]
    public async Task GetTrades_WhenTradesExist_MapsAllToDto()
    {
        // Arrange
        _repo.GetAllByClientIdAsync(Arg.Any<string>()).Returns([
            new Trade()
            {
                ClientId = "abc123",
                Symbol = "ABC",
                Quantity = 1,
                Price = 1
            },
            new Trade()
            {
                ClientId = "abc123",
                Symbol = "EFG",
                Quantity = 2,
                Price = 2
            },
        ]);

        // Act
        var result = await _tradeService.GetTradesForClient("abc123");

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(new OperationResult<List<TradeDto>>()
        {
            Result =
            [
                new TradeDto()
                {
                    ClientId = "abc123",
                    Symbol = "ABC",
                    Quantity = 1,
                    Price = 1
                },
                new TradeDto()
                {
                    ClientId = "abc123",
                    Symbol = "EFG",
                    Quantity = 2,
                    Price = 2
                }
            ]
        });
    }
    
    [Test]
    public void RepoThrowsException_ExceptionIsPropagated()
    {
        // Arrange
        _repo.GetAsync(Arg.Any<Guid>()).Throws(new Exception());
        
        // Act + Assert
        Assert.ThrowsAsync<Exception>(async () => await _tradeService.GetTrade(Guid.NewGuid()));
    }
    
    [Test]
    public void PublisherThrowsException_ExceptionIsPropagated()
    {
        // Arrange
        _publisher.PublishTradeAsync(Arg.Any<Trade>()).Throws(new Exception());
        
        // Act + Assert
        Assert.ThrowsAsync<Exception>(async () => await _tradeService.ExecuteTradeAsync(CreateSampleTradeDto()));
    }

    private TradeDto CreateSampleTradeDto()
    {
        return new TradeDto()
        {
            ClientId = "abc123",
            Symbol = "EUR",
            Price = 100,
            Quantity = 10,
        };
    }
}