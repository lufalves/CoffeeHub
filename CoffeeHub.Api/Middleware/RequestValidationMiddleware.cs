using CoffeeHub.Api.Contracts.Responses;
using System.Text.Json;

namespace CoffeeHub.Api.Middleware;

public sealed class RequestValidationMiddleware(RequestDelegate next)
{
    private static readonly string[] MethodsWithBody = [HttpMethods.Post, HttpMethods.Put, HttpMethods.Patch];

    public async Task InvokeAsync(HttpContext context)
    {
        if (MethodsWithBody.Contains(context.Request.Method, StringComparer.OrdinalIgnoreCase))
        {
            var hasBody = context.Request.ContentLength.GetValueOrDefault() > 0
                || context.Request.Headers.ContainsKey("Transfer-Encoding");

            if (!hasBody)
            {
                await WriteValidationErrorAsync(context, "request.body_required", "Request body is required.");
                return;
            }

            var contentType = context.Request.ContentType;

            if (string.IsNullOrWhiteSpace(contentType) || !contentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            {
                await WriteValidationErrorAsync(context, "request.invalid_content_type", "Content-Type must be application/json.");
                return;
            }
        }

        await next(context);
    }

    private static async Task WriteValidationErrorAsync(HttpContext context, string code, string message)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        var payload = new ErrorResponse(new ErrorDetail(code, message));
        await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
