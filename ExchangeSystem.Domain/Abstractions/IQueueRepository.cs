namespace ExchangeSystem.Domain.Abstractions;

public interface IQueueServiceHandler
{
    Task StartAsync(CancellationToken ct);
    Task SendAsync();
}