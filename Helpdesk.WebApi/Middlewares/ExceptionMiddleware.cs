using Npgsql;
using System.Net;
using Newtonsoft.Json;
using Helpdesk.WebApi.Helpers;

namespace Helpdesk.WebApi.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var messageText = exception.Message;
        if (exception.InnerException is not null and PostgresException)
        {
            messageText = ((PostgresException)exception.InnerException).MessageText;
        }

        var json = JsonConvert.SerializeObject(new
        {
            statusCode = context.Response.StatusCode,
            detail = messageText.ToTitleCase()
        });

        await context.Response.WriteAsync(json);
    }
}