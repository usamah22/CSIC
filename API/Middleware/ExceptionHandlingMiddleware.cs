using Application;
using FluentValidation;

namespace API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var result = exception switch
        {
            ValidationException validationException => new
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Message = "Validation errors occurred",
                Details = validationException.Errors.Select(e => e.ErrorMessage).ToArray()
            },
            NotFoundException notFoundException => new
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = notFoundException.Message,
                Details = Array.Empty<string>()
            },
            UnauthorizedAccessException => new
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = "Unauthorized",
                Details = Array.Empty<string>()
            },
            _ => new
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "An error occurred while processing your request.",
                Details = Array.Empty<string>()
            }
        };

        context.Response.StatusCode = result.StatusCode;
        await context.Response.WriteAsJsonAsync(result);
    }
}