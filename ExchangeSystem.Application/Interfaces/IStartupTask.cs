namespace ExchangeSystem.Application.Interfaces;

public interface IStartupTask
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}