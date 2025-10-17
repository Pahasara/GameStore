using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Middleware;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandler> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandler(
        RequestDelegate next,
        ILogger<ExceptionHandler> logger,
        IHostEnvironment environment
    )
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occured: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private class ErrorResponse
    {
        public HttpStatusCode StatusCode { get; init; }
        public string Title { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
        public string TraceId { get; set; } = string.Empty;
    }

    private ErrorResponse CreateErrorResponse(
        HttpStatusCode statusCode,
        string title,
        string detail
    )
    {
        return new ErrorResponse
        {
            StatusCode = statusCode,
            Title = title,
            Detail = detail,
            TimeStamp = DateTime.UtcNow,
            TraceId = Activity.Current?.Id ?? Guid.NewGuid().ToString()
        };
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            ArgumentNullException => CreateErrorResponse(
                HttpStatusCode.BadRequest,
                "Invalid Request",
                "A required parameter was null or empty"),
            ArgumentException => CreateErrorResponse(
                HttpStatusCode.BadRequest,
                "Invalid Request",
                exception.Message),
            KeyNotFoundException => CreateErrorResponse(
                HttpStatusCode.NotFound,
                "Resource Not Found",
                exception.Message),
            UnauthorizedAccessException => CreateErrorResponse(
                HttpStatusCode.Unauthorized,
                "Unauthorized",
                exception.Message),
            DbUpdateException => CreateErrorResponse(
                HttpStatusCode.BadRequest,
                "Database Error",
                "Failed to update teh database. Please check your input."),
            InvalidOperationException => CreateErrorResponse(
                HttpStatusCode.BadRequest,
                "Invalid Operation",
                exception.Message),
            _ => CreateErrorResponse(
                HttpStatusCode.InternalServerError,
                "Internal Server Error",
                _environment.IsDevelopment()
                    ? exception.Message
                    : "An error occured while processing your request.")
        };

        context.Response.StatusCode = (int)response.StatusCode;

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = _environment.IsDevelopment()
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}
