namespace ExchangeSystem.Application.Models;

public class TradeDto
{
    public string ClientId { get; init; }
    public string Symbol { get; init; }
    public int Quantity { get; init; }
    public decimal Price { get; init; }
}