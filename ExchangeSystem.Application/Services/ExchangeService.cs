using ExchangeSystem.Application.Interfaces.InfrastructureInterfaces;
using ExchangeSystem.Application.Models;

namespace ExchangeSystem.Application.Services;

public class ExchangeService : ExchangeSystem.Application.Interfaces.IExchangeService
{
    private readonly IExchangeService _exchangeService;
    
    public ExchangeService(IExchangeService exchangeService)
    {
        _exchangeService = exchangeService;
    }
    
    public async Task ExecuteTrade(ExecuteTradeRequest request)
    {
        await _exchangeService.ExecuteTrade(request);
    }
}