using MassTransit;

namespace Elfland.Ocean.EventBus.Events;

public class IntegrationEvent
{
    public IntegrationEvent()
    {
        Id = NewId.Next();
        CreationDate = DateTime.UtcNow;
    }

    public NewId Id { get; private set; }

    public DateTime CreationDate { get; private set; }
}
