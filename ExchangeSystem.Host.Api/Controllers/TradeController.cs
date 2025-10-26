using ExchangeSystem.Application.Exceptions;
using ExchangeSystem.Application.Interfaces;
using ExchangeSystem.Application.Models;
using ExchangeSystem.Host.Api.Contracts.Request;
using ExchangeSystem.Host.Api.Contracts.Response;
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

    [HttpGet("{tradeId}")]
    public async Task<IActionResult> GetTrade([FromHeader] Guid correlationId, [FromRoute] Guid tradeId)
    {
        using (_logger.BeginScope(new Dictionary<string, object?>
               {
                   ["CorrelationId"] = correlationId,
               }))
        {
            _logger.LogInformation("Get trade request received with tradeId: {@request}", tradeId);

            try
            {
                var tradeOperation = await _tradeService.GetTrade(tradeId);

                if (tradeOperation.Result == null)
                {
                    return NotFound();
                }

                return Ok(tradeOperation.Result);
            }
            catch (RetrieveTradeException)
            {
                return StatusCode(502);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unknown exception occured");
            }
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetTrades([FromHeader] Guid correlationId)
    {
        using (_logger.BeginScope(new Dictionary<string, object?>
               {
                   ["CorrelationId"] = correlationId,
               }))
        {
            _logger.LogInformation("Get trades request received;");
            try
            {
                var trades = await _tradeService.GetTrades();

                return Ok(trades);
            }
            catch (RetrieveTradeException)
            {
                return StatusCode(502);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unknown exception occured");
            }
        }
    }


    [HttpPost]
    public async Task<IActionResult> CreateTrade([FromHeader] Guid correlationId, [FromBody] TradeRequest request)
    {
        using (_logger.BeginScope(new Dictionary<string, object?>
               {
                   ["CorrelationId"] = correlationId,
               }))
        {
            _logger.LogInformation("Trade request received {@request}", request);

            var applicationRequest = new TradeDto()
            {
                ClientId = request.ClientId,
                Symbol = request.Symbol,
                Quantity = request.Quantity,
                Price = request.Price,
            };

            try
            {
                var executeTradeOperation = await _tradeService.ExecuteTradeAsync(applicationRequest);

                return Ok(new ExecuteTradeResponse()
                {
                    TradeId = executeTradeOperation.Result
                });
            }
            catch (SaveTradeFailedException)
            {
                return StatusCode(502);
            }
            catch (PublishTradeFailedException)
            {
                return StatusCode(502);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unknown error occured");
            }
        }
    }
}