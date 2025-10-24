using System.Text;
using ExchangeSystem.Domain.Abstractions;
using RabbitMQ.Client;

namespace ExchangeSystem.Infrastructure.Rabbit;

public sealed class RabbitMqBase: IQueueServiceHandler, IAsyncDisposable
{
    private readonly ConnectionFactory _factory;
    private IConnection _connection;
    private IChannel _channel;

    public RabbitMqBase()
    {
       _factory = new ConnectionFactory { HostName = "localhost" };
    }
    
    public async Task StartAsync(CancellationToken ct)
    {
        _connection = await _factory.CreateConnectionAsync(ct);
        _channel = await _connection.CreateChannelAsync(cancellationToken: ct);
         
        await _channel.QueueDeclareAsync(queue: "hello", durable: false, exclusive: false, autoDelete: false,
            arguments: null, cancellationToken: ct);
    }
    
    public async Task SendAsync()
    {
        const string message = "Hello World!";
        var body = Encoding.UTF8.GetBytes(message);
    
        await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
        Console.WriteLine($" [x] Sent {message}");
    }

    public async ValueTask DisposeAsync()
    {
        Console.WriteLine("Disposing RabbitMqBase");

        await _connection.CloseAsync();
        await _channel.DisposeAsync();
    }
}