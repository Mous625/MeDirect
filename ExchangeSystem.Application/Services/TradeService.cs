using ExchangeSystem.Application.Interfaces;
using ExchangeSystem.Application.Models;
using ExchangeSystem.Domain.Abstractions;
using ExchangeSystem.Domain.Entities;

namespace ExchangeSystem.Application.Services;

public class TradeService : ITradeService
{
    private readonly ITradeRepository _tradeRepository;
    private readonly IQueuePublisher _queuePublisher;
    
    public TradeService(ITradeRepository tradeRepository, IQueuePublisher queueRepository)
    {
        _tradeRepository = tradeRepository;
        _queuePublisher = queueRepository;
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
        
        await _tradeRepository.AddAsync(trade);
        await _queuePublisher.PublishTradeAsync(trade);
    }
}