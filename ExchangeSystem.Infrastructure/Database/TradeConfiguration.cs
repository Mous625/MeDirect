using ExchangeSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeSystem.Infrastructure.Database;

public sealed class TradeConfiguration : IEntityTypeConfiguration<Trade>
{
    public void Configure(EntityTypeBuilder<Trade> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.ClientId).IsRequired().HasMaxLength(20);
        b.Property(x => x.Symbol).IsRequired().HasMaxLength(20);
        b.Property(x => x.Quantity).IsRequired();
        b.Property(x => x.Price).HasColumnType("decimal(18,2)").IsRequired();
        b.Property(x => x.ExecutedAt);
    }
}