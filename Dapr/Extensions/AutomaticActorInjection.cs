using Dapr.Actors.Runtime;

namespace Elfland.Dapr.Extensions;

public static class AutomaticActorInjection
{
    /// <summary>
    /// Registers all actors inherited from Actor type in the collection.
    /// </summary>
    /// <param name="actorRegistrationCollection">A collection of ActorRegistration instances.</param>
    public static void RegisterActors(this ActorRegistrationCollection actorRegistrationCollection)
    {
        var genericMethodInfo = actorRegistrationCollection.GetType().GetMethod("RegisterActor");

        AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(
                implementationType =>
                    implementationType.IsSubclassOf(typeof(Actor))
                    && implementationType.IsClass
                    && !implementationType.IsAbstract
            )
            .ToList()
            .ForEach(
                implementationType =>
                    genericMethodInfo
                        ?.MakeGenericMethod(implementationType)
                        ?.Invoke(actorRegistrationCollection, new object?[] { null })
            );
    }
}
