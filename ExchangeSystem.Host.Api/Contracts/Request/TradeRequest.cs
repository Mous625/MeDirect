namespace ExchangeSystem.Host.Api.Contracts.Request;

public class TradeRequest
{
    public Guid CorrelationId { get; init; }
    public required string ClientId { get; init; }
    public required string Symbol { get; init; }
    public required int Quantity { get; init; }
    public required decimal Price { get; init; }
}