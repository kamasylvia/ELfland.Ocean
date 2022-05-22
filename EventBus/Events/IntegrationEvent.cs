using MassTransit;

namespace Elfland.Ocean.EventBus.Events;

public class IntegrationEvent
{
    public Guid? Id { get; private set; } = NewId.NextGuid();

    public DateTime? CreationDate { get; private set; } = DateTime.UtcNow;
}
