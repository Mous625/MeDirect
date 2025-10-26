using ExchangeSystem.Application.Interfaces;
using ExchangeSystem.Application.Models;
using ExchangeSystem.Host.Api.Contracts.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExchangeSystem.Host.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TradeController : ControllerBase
{
    private readonly ITradeService _tradeService;
    private readonly ILogger<TradeController> _logger;

    public TradeController(ITradeService tradeService, ILogger<TradeController> logger)
    {
        _tradeService = tradeService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTrade([FromBody] TradeRequest request)
    {
        using (_logger.BeginScope(new Dictionary<string, object?>
               {
                   ["CorrelationId"] = request.CorrelationId,
               }))
        {
            _logger.LogInformation("Trade request received {@request}", request);
        
            var applicationRequest = new ExecuteTradeRequest()
            {
                ClientId = request.ClientId,
                Symbol = request.Symbol,
                Quantity = request.Quantity,
                Price = request.Price,
            };
        
            await _tradeService.ExecuteTrade(applicationRequest);
        }        
        
        return NoContent();
    }
}