using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqConsumer;

var host = Environment.GetEnvironmentVariable("RabbitMq__HostName") ?? "localhost";
var exchange = Environment.GetEnvironmentVariable("RabbitMq__ExchangeName") ?? "trades";
var exType = Environment.GetEnvironmentVariable("RabbitMq__ExchangeType") ?? "fanout";
var queue = Environment.GetEnvironmentVariable("RabbitMq__QueueName") ?? "trades";

var rabbitMqOptions = new RabbitMqOptions
{
    HostName = host,
    ExchangeName = exchange,
    QueueName = queue,
    ExchangeType = exType,
    RoutingKey = ""
};

using var cts = new CancellationTokenSource();

// Keep alive mechanism for docker
Console.CancelKeyPress += (s, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

AppDomain.CurrentDomain.ProcessExit += (s, e) => cts.Cancel();

var factory = new ConnectionFactory { HostName = rabbitMqOptions.HostName };
await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: rabbitMqOptions.ExchangeName, durable: true,
    type: rabbitMqOptions.ExchangeType);

var consumer = new AsyncEventingBasicConsumer(channel);

Console.WriteLine("Consumer: Waiting for logs.");

consumer.ReceivedAsync += (model, ea) =>
{
    byte[] body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Consumer Received: {message}");
    return Task.CompletedTask;
};

await channel.BasicConsumeAsync(rabbitMqOptions.QueueName, autoAck: true, consumer: consumer);

await Task.Delay(Timeout.Infinite, cts.Token);
