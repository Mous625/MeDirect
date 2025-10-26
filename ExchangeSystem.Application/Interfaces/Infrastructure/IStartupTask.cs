namespace ExchangeSystem.Application.Interfaces.Infrastructure;

public interface IStartupTask
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}