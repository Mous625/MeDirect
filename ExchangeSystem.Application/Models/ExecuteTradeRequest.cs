namespace ExchangeSystem.Application.Models;

public class ExecuteTradeRequest
{
    public string ClientId { get; init; }
    public string Symbol { get; init; }
    public string Quantity { get; init; }
    public string Price { get; init; }
}