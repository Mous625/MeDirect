using ExchangeSystem.Domain.Abstractions;

namespace ExchangeSystem;

public sealed class StartupWorker : BackgroundService
{
    private readonly IQueueServiceHandler _queue;

    public StartupWorker(IQueueServiceHandler queue)
    {
        _queue = queue;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        await _queue.StartAsync(ct);
    }
}
