using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Elfland.WebAPI.Exceptions;

public class ConflictException : HttpExceptionBase
{
    public override int? StateCode { get; set; } = StatusCodes.Status409Conflict;

    public ConflictException() { }

    public ConflictException(string message) : base(message) { }

    public ConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
    { }
}
