using ExchangeSystem.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeSystem.Infrastructure.Database;

public class TradeContext : DbContext
{
    private DbSet<Trade> Trades { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=localhost;user=root;password=1234;database=ef", ServerVersion.AutoDetect("server=localhost;user=root;password=1234;database=ef"));
    }

    public void SaveTrade(Trade trade)
    {
        Trades.Add(trade);
        SaveChangesAsync();
    }
}

