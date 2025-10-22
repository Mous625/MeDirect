using ExchangeSystem.Domain.Abstractions;
using ExchangeSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExchangeSystem.Infrastructure.Database;

public class TradeContext : DbContext, ITradeRepository
{
    private DbSet<Trade> Trades => Set<Trade>();

    public TradeContext(DbContextOptions<TradeContext> options)
        : base(options) { }
    
    public async Task AddAsync(Trade trade)
    {
        try
        {
            Trades.Add(trade);
            await SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}

