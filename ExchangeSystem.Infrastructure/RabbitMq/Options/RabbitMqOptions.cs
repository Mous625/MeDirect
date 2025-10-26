using System.ComponentModel.DataAnnotations;

namespace ExchangeSystem.Infrastructure.RabbitMq.Options;

internal sealed class RabbitMqOptions
{
    [Required] public required string ConnectionString { get; init; }

    [Required] public required string ExchangeName { get; init; }

    [Required] public required string ExchangeType { get; init; }

    [Required] public required string QueueName { get; init; }

    [Required] public required string RoutingKey { get; init; }
}