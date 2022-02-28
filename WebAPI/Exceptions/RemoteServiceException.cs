using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Elfland.WebAPI.Exceptions;

public class RemoteServiceException : HttpExceptionBase
{
    public override int? StateCode { get; set; } = StatusCodes.Status503ServiceUnavailable;

    public RemoteServiceException() { }

    public RemoteServiceException(string message) : base(message) { }

    public RemoteServiceException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
