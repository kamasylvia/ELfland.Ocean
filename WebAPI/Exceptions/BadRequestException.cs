using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Elfland.WebAPI.Exceptions;

public class BadRequestException : HttpExceptionBase
{
    public override int? StateCode { get; set; } = StatusCodes.Status400BadRequest;

    public BadRequestException() { }

    public BadRequestException(string message) : base(message) { }

    public BadRequestException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
