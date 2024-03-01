using System.Diagnostics;
using System.Net;
using System.Text;
using Tree.Services.Contracts;
using Tree.Services.Exceptions;
using Tree.Services.Models;
using Trees.Exceptions;

namespace Trees;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogsService _logsService;

    public ExceptionMiddleware(RequestDelegate next, ILogsService logsService)
    {
        _next = next;
        _logsService = logsService;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            httpContext.Request.EnableBuffering();
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(httpContext, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var traceId = Activity.Current?.Id ?? Guid.NewGuid().ToString("D");
        var result = exception switch
        {
            SecureException => new ApiError(traceId, "Secure", exception),
            OperationCanceledException => new ApiError(traceId, "OperationCanceled", "Operation was cancelled"),
            not null => new ApiError(traceId),
            _ => null
        };

        if (result != null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(result);
        }

        var requestUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
        var rawRequestBody = await GetBodyAsync(context.Request);

        await _logsService.AddLog(new LogAddModel{ TraceId = traceId, Text =  $"""
                                                                         Request: {requestUrl}
                                                                         Content: {rawRequestBody}
                                                                         Exception: {exception}
                                                                         """});
    }

    private static async Task<string> GetBodyAsync(HttpRequest request, Encoding? encoding = null)
    {
        request.Body.Position = 0;
        using var reader = new StreamReader(request.Body, encoding ?? Encoding.UTF8);
        var body = await reader.ReadToEndAsync().ConfigureAwait(false);
        request.Body.Position = 0;

        return body;
    }
}
