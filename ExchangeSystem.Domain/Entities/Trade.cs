using System.ComponentModel.DataAnnotations;

namespace ExchangeSystem.Domain.Entities;

public class Trade
{
    [Key]
    public Guid TradeId { get; set; }
    public string ClientId { get; init; }
    public string Symbol { get; init; }
    public string Quantity { get; init; }
    public string Price { get; init; }
}