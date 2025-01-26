namespace API.Middleware;

public static class SwaggerVersionMiddlewareExtensions
{
    public static IApplicationBuilder UseSwaggerVersion(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SwaggerVersionMiddleware>();
    }
}