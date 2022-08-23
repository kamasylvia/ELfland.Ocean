using MassTransit;

namespace Elfland.Ocean.EventBus.Events;

public record class IntegrationEvent
{
    public Guid? Id { get; private set; } = NewId.NextGuid();

    public DateTime? CreationDate { get; init; } = DateTime.UtcNow;
}
