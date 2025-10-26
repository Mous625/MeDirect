using ExchangeSystem.Domain.Abstractions;
using ExchangeSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExchangeSystem.Infrastructure.Database;

public class TradeContext : DbContext, ITradeRepository
{
    private DbSet<Trade> Trades => Set<Trade>();
    private readonly ILogger<TradeContext> _logger;

    public TradeContext(DbContextOptions<TradeContext> options, ILogger<TradeContext> logger)
        : base(options)
    {
        _logger = logger;
    }
    
    public async Task AddAsync(Trade trade)
    {
        try
        {
            Trades.Add(trade);
            await SaveChangesAsync();
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured whilst creating a new trade.");
        }
    }
}

