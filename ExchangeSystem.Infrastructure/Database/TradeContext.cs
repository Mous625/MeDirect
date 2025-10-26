using ExchangeSystem.Application.Exceptions;
using ExchangeSystem.Application.Interfaces;
using ExchangeSystem.Application.Interfaces.Infrastructure;
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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TradeContext).Assembly);
    }
    
    public async Task<Trade?> GetAsync(Guid tradeId)
    {
        try
        {
            var trade = await Trades.FindAsync(tradeId);
            return trade;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured whilst creating a new trade.");
            throw new RetrieveTradeException();
        }
    }
    
    public async Task<List<Trade>> GetAllAsync()
    {
        try
        {
            return await Trades.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured whilst creating a new trade.");
            throw new RetrieveTradeException();
        }
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
            throw new PublishTradeFailedException();
        }
    }
}

