namespace ExchangeSystem.Host.Api.Models.Request;

public class TradeRequest
{
    public string ClientId { get; init; }
    public string Symbol { get; init; }
    public string Quantity { get; init; }
    public string Price { get; init; }
}