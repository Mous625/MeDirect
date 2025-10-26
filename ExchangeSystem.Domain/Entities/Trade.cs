using System.ComponentModel.DataAnnotations;

namespace ExchangeSystem.Domain.Entities;

public class Trade
{
    [Key]
    public Guid TradeId { get; init; }
    public required string ClientId { get; init; }
    public required string Symbol { get; init; }
    public required int Quantity { get; init; }
    public required decimal Price { get; init; }
}