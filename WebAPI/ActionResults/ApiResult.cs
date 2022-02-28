using Microsoft.AspNetCore.Mvc;

namespace Elfland.WebAPI.ActionResults;

public class ApiResult : ActionResult
{
    public int? StatusCode { get; set; }

    public object? Data { get; set; }

    public object? Errors { get; set; }

    public bool Succeeded { get; set; }
}
