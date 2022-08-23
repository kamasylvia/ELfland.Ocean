using Elfland.Ocean.EventBus.Events;

namespace Elfland.Ocean.EventBus.Abstractions;

public interface IEventBus
{
    Task PublishAsync<TIntegrationEvent>(TIntegrationEvent @event, string daprPubsubName = "pubsub")
        where TIntegrationEvent : IntegrationEvent;
}
