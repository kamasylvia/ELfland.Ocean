using Grpc.Core;

namespace Elfland.Dapr.Exceptions;

public class NotFoundGrpcException : RpcException
{
    public NotFoundGrpcException()
        : base(
            new Status(
                StatusCode.NotFound,
                "Some requested entity (e.g., file or directory) was not found. "
            )
        ) { }

    public NotFoundGrpcException(string message) : base(new Status(StatusCode.NotFound, message))
    { }
}
