using ExchangeSystem.Application.Exceptions;
using ExchangeSystem.Application.Interfaces;
using ExchangeSystem.Application.Models;
using ExchangeSystem.Host.Api.Contracts.Request;
using ExchangeSystem.Host.Api.Contracts.Response;
using ExchangeSystem.Host.Api.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace ExchangeSystem.UnitTests;

public class TradesControllerTests
{
    private TradesController _controller;
    private ITradeService _tradeService;
    private ILogger<TradesController> _logger;

    [SetUp]
    public void Setup()
    {
        _tradeService = Substitute.For<ITradeService>();
        _logger = Substitute.For<ILogger<TradesController>>();

        _controller = new TradesController(_tradeService, _logger);
    }

    [Test]
    public async Task GetTrade_WhenMissing_Returns404()
    {
        // Arrange
        _tradeService.GetTrade(Arg.Any<Guid>()).Returns(new OperationResult<TradeDto?>()
        {
            Result = null,
        });

        // Act
        var result = await _controller.GetTrade(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task GetTrade_WhenRetrieveTradeException_Returns502()
    {
        // Arrange
        _tradeService.GetTrade(Arg.Any<Guid>()).Throws(new RetrieveTradeException());

        // Act
        var result = await _controller.GetTrade(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        result.Should().BeOfType<StatusCodeResult>()
            .Which.StatusCode.Should().Be(502);
    }

    [Test]
    public async Task GetTrade_WhenUnknownException_Returns500()
    {
        // Arrange
        _tradeService.GetTrade(Arg.Any<Guid>()).Throws(new Exception());

        // Act
        var result = await _controller.GetTrade(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        result.Should().BeOfType<ObjectResult>()
            .Which.StatusCode.Should().Be(500);
    }
    
    [Test]
    public async Task GetTrade_WhenOk_Returns200_WithTradeResponse()
    {
        // Arrange
        _tradeService.GetTrade(Arg.Any<Guid>()).Returns(new OperationResult<TradeDto?> { Result = CreateSampleTradeDto() });

        // Act
        var result = await _controller.GetTrade(Guid.NewGuid(), Guid.Empty);

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<TradeResponse>();
        result.Should().BeOfType<OkObjectResult>().Which.Value.Should()
            .BeEquivalentTo(CreateSampleTradeResponse());
    }

    [Test]
    public async Task GetTrades_WhenOk_Returns200_WithTradeResponse()
    {
        // Arrange
        var list = new List<TradeDto>
        {
            CreateSampleTradeDto()
        };

        _tradeService.GetTradesForClient(Arg.Any<string>())
            .Returns(new OperationResult<List<TradeDto>> { Result = list });

        // Act
        var result = await _controller.GetTradesForClient(Guid.NewGuid(), string.Empty);

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<List<TradeResponse>>();
        result.Should().BeOfType<OkObjectResult>().Which.Value.Should()
            .BeEquivalentTo(new List<TradeResponse> { CreateSampleTradeResponse() });
    }
    
    [Test]
    public async Task CreateTrade_WhenValid_Returns200_WithTradeId()
    {
        // Arrange
        var req = new TradeRequest { ClientId = "123abc", Symbol = "ABC", Quantity = 1, Price = 10 };
        _tradeService.ExecuteTradeAsync(Arg.Any<TradeDto>())
            .Returns(new OperationResult<string?> { Result = "guid" });

        // Act
        var result = await _controller.CreateTrade(Guid.NewGuid(), req);

        // Assert
        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<ExecuteTradeResponse>();
        result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(new ExecuteTradeResponse()
        {
            TradeId = "guid"
        });
    }

    private TradeDto CreateSampleTradeDto()
    {
        return new TradeDto()
        {
            TradeId = Guid.Parse("08de1570-c8e9-4276-88aa-947507573ea4"),
            ClientId = "abc123",
            Symbol = "EUR",
            Price = 100,
            Quantity = 10,
        };
    }

    private TradeResponse CreateSampleTradeResponse()
    {
        return new TradeResponse()
        {
            TradeId = Guid.Parse("08de1570-c8e9-4276-88aa-947507573ea4"),
            ClientId = "abc123",
            Symbol = "EUR",
            Price = 100,
            Quantity = 10,
        };
    }
}