namespace ExchangeSystem.Infrastructure.Models;

public class Trade
{
    public string ClientId { get; init; }
    public string Symbol { get; init; }
    public string Quantity { get; init; }
    public string Price { get; init; }
}