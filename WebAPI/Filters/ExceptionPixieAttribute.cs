using Dapr;
using Dapr.Actors;
using Dapr.Client;
using Elfland.WebAPI.ActionResults;
using Elfland.WebAPI.Exceptions;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Elfland.WebAPI.Filters;

public class ExceptionPixieAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<ExceptionPixieAttribute> _logger;

    public ExceptionPixieAttribute(ILogger<ExceptionPixieAttribute> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override void OnException(ExceptionContext context)
    {
        string methodInfo =
            $"{context.RouteData.Values["controller"] as string}Controller.{context.RouteData.Values["action"] as string}:{context.HttpContext.Request.Method}";

        _logger.LogError(
            context.Exception,
            $"Unhandled exception occurred while executing request: {methodInfo}"
        );

        var apiResult = new ApiResult { Succeeded = false };

        ExceptionHandler(context.Exception, apiResult);

        _logger.LogError(apiResult.Errors as string);
        context.Result = new ObjectResult(apiResult) { StatusCode = apiResult.StatusCode };
    }

    private void ExceptionHandler(Exception? exception, ApiResult apiResult)
    {
        switch (exception)
        {
            case BadRequestException ex:
                apiResult.StatusCode = ex.StateCode;
                apiResult.Errors = ex.Message ?? "Bad request error.";
                break;
            case NotFoundException ex:
                apiResult.StatusCode = ex.StateCode;
                apiResult.Errors = ex.Message ?? "Could not find the entity.";
                break;
            case DatabaseUpdateException ex:
                apiResult.StatusCode = ex.StateCode;
                apiResult.Errors = ex.Message ?? "Database Update Error.";
                break;
            case RemoteServiceException ex:
                apiResult.StatusCode = ex.StateCode;
                apiResult.Errors = ex.Message ?? "Remote service error.";
                break;
            case ConflictException ex:
                apiResult.StatusCode = ex.StateCode;
                apiResult.Errors = ex.Message ?? "Conflict error.";
                break;
            case ActorInvokeException ex:
                apiResult.StatusCode = StatusCodes.Status500InternalServerError;
                apiResult.Errors = ex.ActualExceptionType;
                break;
            // DaprException
            case ActorMethodInvocationException ex:
                if (ex.InnerException is not DaprException)
                {
                    ExceptionHandler(ex?.InnerException, apiResult);
                }
                break;
            // DaprException
            case InvocationException ex:
                switch (ex.InnerException)
                {
                    case RpcException rpcEx:
                        switch (rpcEx.StatusCode)
                        {
                            case StatusCode.NotFound:
                                apiResult.StatusCode = StatusCodes.Status404NotFound;
                                apiResult.Errors = rpcEx.Message;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        apiResult.StatusCode = StatusCodes.Status503ServiceUnavailable;
                        apiResult.Errors = ex.InnerException?.Message ?? "Server Unavailable Error";
                        break;
                }
                break;
            default:
                apiResult.StatusCode = StatusCodes.Status500InternalServerError;
                apiResult.Errors = "Internal Server Error.";
                break;
        }
    }
}
