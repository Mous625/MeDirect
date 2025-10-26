using ExchangeSystem.Application.Interfaces;
using ExchangeSystem.Application.Models;
using ExchangeSystem.Host.Api.Contracts.Request;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeSystem.Host.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TradeController : ControllerBase
{
    private readonly ITradeService _tradeService;

    public TradeController(ITradeService tradeService)
    {
        _tradeService = tradeService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTrade([FromBody] TradeRequest request)
    {
        var applicationRequest = new ExecuteTradeRequest()
        {
            ClientId = request.ClientId,
            Symbol = request.Symbol,
            Quantity = request.Quantity,
            Price = request.Price,
        };
        
        await _tradeService.ExecuteTrade(applicationRequest);
        
        return NoContent();
    }
}