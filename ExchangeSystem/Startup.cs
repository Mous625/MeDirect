using ExchangeSystem.Application.Interfaces;

namespace ExchangeSystem;

public class Startup : IHostedService
{
    private readonly IStartupTask _startupTask;
    
    public Startup(IStartupTask startupTask)
    {
        _startupTask = startupTask;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _startupTask.ExecuteAsync(cancellationToken);
    }
    
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}