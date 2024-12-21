namespace MA.SlotService.Application.Abstractions;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent e, CancellationToken ct);
}