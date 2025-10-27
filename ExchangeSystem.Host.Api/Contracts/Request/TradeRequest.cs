using System.ComponentModel.DataAnnotations;

namespace ExchangeSystem.Host.Api.Contracts.Request;

public class TradeRequest
{
    public required string ClientId { get; init; }
    public required string Symbol { get; init; }
    
    [Range(1, 100)]
    public required int Quantity { get; init; }
    
    [Range(1, double.MaxValue)]
    public required decimal Price { get; init; }
}