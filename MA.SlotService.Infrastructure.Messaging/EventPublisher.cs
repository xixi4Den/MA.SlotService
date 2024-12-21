using System.Text.Json;
using MA.SlotService.Application.Abstractions;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace MA.SlotService.Infrastructure.Messaging;

public class EventPublisher(IBus bus, ILogger<EventPublisher> logger) : IEventPublisher
{
    public async Task PublishAsync<TEvent>(TEvent e, CancellationToken ct)
    {
        logger.LogDebug("Publish {Type} event: {Message}", typeof(TEvent), JsonSerializer.Serialize(e));
        
        await bus.Publish(e, ct);
    }
}