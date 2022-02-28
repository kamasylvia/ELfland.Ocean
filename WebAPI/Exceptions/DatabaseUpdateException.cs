using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace Elfland.WebAPI.Exceptions;

public class DatabaseUpdateException : HttpExceptionBase
{
    public override int? StateCode { get; set; } = StatusCodes.Status500InternalServerError;

    public DatabaseUpdateException() { }

    public DatabaseUpdateException(string message) : base(message) { }

    public DatabaseUpdateException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
