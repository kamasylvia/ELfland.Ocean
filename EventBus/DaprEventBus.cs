using Dapr.Client;
using Elfland.Ocean.EventBus.Abstractions;
using Elfland.Ocean.EventBus.Events;
using Microsoft.Extensions.Logging;

namespace Elfland.Ocean.EventBus
{
    public class DaprEventBus : IEventBus
    {
        private readonly DaprClient _dapr;
        private readonly ILogger _logger;

        public DaprEventBus(DaprClient dapr, ILogger<DaprEventBus> logger)
        {
            _dapr = dapr ?? throw new ArgumentNullException(nameof(dapr));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PublishAsync<TIntegrationEvent>(
            TIntegrationEvent @event,
            string daprPubsubName = "pubsub"
        ) where TIntegrationEvent : IntegrationEvent
        {
            var topicName = @event.GetType().Name;

            _logger.LogInformation($"Publishing event {@event} to {daprPubsubName}.{topicName}");

            // We need to make sure that we pass the concrete type to PublishEventAsync,
            // which can be accomplished by casting the event to dynamic. This ensures
            // that all event fields are properly serialized.
            await _dapr.PublishEventAsync(daprPubsubName, topicName, (object)@event);
        }
    }
}
