using ExchangeSystem.Application.Interfaces;
using ExchangeSystem.Application.Interfaces.Infrastructure;
using ExchangeSystem.Application.Models;
using ExchangeSystem.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace ExchangeSystem.Application.Services;

public class TradeService : ITradeService
{
    private readonly ITradeRepository _tradeRepository;
    private readonly IQueuePublisher _queuePublisher;
    private readonly ILogger<TradeService> _logger;
    public TradeService(ITradeRepository tradeRepository, IQueuePublisher queueRepository, ILogger<TradeService> logger)
    {
        _tradeRepository = tradeRepository;
        _queuePublisher = queueRepository;
    }
    
    public async Task<OperationResult<string?>> ExecuteTradeAsync(TradeDto dto)
    {
        var trade = new Trade
        {
            ClientId = dto.ClientId,
            Price = dto.Price,
            Quantity = dto.Quantity,
            Symbol = dto.Symbol
        };
        
        await _tradeRepository.AddAsync(trade);
        await _queuePublisher.PublishTradeAsync(trade);
        
        return new OperationResult<string?> { Result = trade.Id.ToString() };
    }

    public async Task<OperationResult<TradeDto?>> GetTrade(Guid tradeId)
    {
        var trade = await _tradeRepository.GetAsync(tradeId);

        var tradeDto = trade != null
            ? new TradeDto
            {
                TradeId = trade.Id,
                ClientId = trade.ClientId,
                Price = trade.Price,
                Quantity = trade.Quantity,
                Symbol = trade.Symbol
            }
            : null;

        return new OperationResult<TradeDto?> { Result = tradeDto };
    }

    public async Task<OperationResult<List<TradeDto>>> GetTradesForClient(string clientId)
    {
        var trades = await _tradeRepository.GetAllByClientIdAsync(clientId);
            
        List<TradeDto> tradesDto = trades.Select(trade => new TradeDto
        {
            TradeId = trade.Id,
            ClientId = trade.ClientId,
            Price = trade.Price,
            Quantity = trade.Quantity,
            Symbol = trade.Symbol
        }).ToList();
        
        return new OperationResult<List<TradeDto>> { Result = tradesDto };
    }
}