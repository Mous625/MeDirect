using ExchangeSystem.Application.Interfaces;
using ExchangeSystem.Application.Models;
using ExchangeSystem.Host.Api.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeSystem.Host.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ExchangeController : ControllerBase
{
    private readonly IExchangeService _exchangeService;

    public ExchangeController(IExchangeService exchangeService)
    {
        _exchangeService = exchangeService;
    }

    [HttpPost]
    public IActionResult GetWeatherForecast([FromBody] TradeRequest request)
    {
        var applicationRequest = new ExecuteTradeRequest()
        {
            ClientId = request.ClientId,
            Symbol = request.Symbol,
            Quantity = request.Quantity,
            Price = request.Price,
        };
        
        _exchangeService.ExecuteTrade(applicationRequest);
        
        return Ok(request.Quantity);
    }
}