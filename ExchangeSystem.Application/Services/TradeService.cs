using ExchangeSystem.Application.Interfaces;
using ExchangeSystem.Application.Models;
using ExchangeSystem.Domain.Abstractions;
using ExchangeSystem.Domain.Entities;

namespace ExchangeSystem.Application.Services;

public class TradeService : ITradeService
{
    private readonly ITradeRepository _tradeRepository;
    private readonly IQueueServiceHandler _queueRepository;
    
    public TradeService(ITradeRepository tradeRepository, IQueueServiceHandler queueRepository)
    {
        _tradeRepository = tradeRepository;
        _queueRepository = queueRepository;
    }
    
    public async Task ExecuteTrade(ExecuteTradeRequest request)
    {
        var trade = new Trade
        {
            ClientId = request.ClientId,
            Price = request.Price,
            Quantity = request.Quantity,
            Symbol = request.Symbol
        };
        
        // await _tradeRepository.AddAsync(trade);
        await _queueRepository.SendAsync();
    }
}