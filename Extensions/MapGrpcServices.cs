using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using static Dapr.AppCallback.Autogen.Grpc.v1.AppCallback;

namespace Elfland.Ocean.Extensions;

public static partial class ProgramExtensions
{
    /// <summary>
    /// Add all gRPC services in the assembly to the application.
    /// </summary>
    /// <param name="app"></param>
    public static void MapGrpcServices(this WebApplication app) =>
        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapGrpcServices();
            }
        );

    private static void MapGrpcServices(this IEndpointRouteBuilder endpoints) =>
        AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(
                implementationType =>
                    implementationType.IsSubclassOf(typeof(AppCallbackBase))
                    && implementationType.IsClass
                    && !implementationType.IsAbstract
            )
            .ToList()
            .ForEach(
                implementationType =>
                    endpoints
                        .GetType()
                        .GetMethod("MapGrpcService", BindingFlags.Public)
                        ?.MakeGenericMethod(implementationType)
                        ?.Invoke(endpoints, new object?[] { null })
            );
}
