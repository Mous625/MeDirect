using ExchangeSystem.Infrastructure.RabbitMq.Interfaces;
using ExchangeSystem.Infrastructure.RabbitMq.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ExchangeSystem.Infrastructure.RabbitMq;

public sealed class RabbitMqBase: IChannelProvider, IAsyncDisposable
{
    private readonly ConnectionFactory _factory;
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqBase(IOptions<RabbitMqOptions> rabbitMqOptions)
    {
       _factory = new ConnectionFactory { HostName = rabbitMqOptions.Value.ConnectionString };
    }
    
    public async Task<IChannel> CreateAndGetChannelIfNotExistsAsync(CancellationToken ct)
    {
        if (_connection is not { IsOpen: true })
        {
            _connection = await _factory.CreateConnectionAsync(ct);
        }

        if (_channel is not { IsOpen: true })
        {
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);
        }
        
        return _channel;
    }
    
    public IChannel GetChannel() => _channel;
    
    public async ValueTask DisposeAsync()
    {
        if (_connection is { IsOpen: true })
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }

        if (_channel is { IsOpen: true })
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }
    }
}