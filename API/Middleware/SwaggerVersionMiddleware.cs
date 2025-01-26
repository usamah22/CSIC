// API/Middleware/SwaggerVersionMiddleware.cs

using System.Text.Json;

public class SwaggerVersionMiddleware
{
    private readonly RequestDelegate _next;

    public SwaggerVersionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.Value?.StartsWith("/swagger/v1/swagger.json") == true)
        {
            context.Response.ContentType = "application/json";
            
            var originalBody = context.Response.Body;
            using var memStream = new MemoryStream();
            context.Response.Body = memStream;

            await _next(context);

            memStream.Position = 0;
            using var reader = new StreamReader(memStream);
            var swaggerJson = await reader.ReadToEndAsync();

            using JsonDocument document = JsonDocument.Parse(swaggerJson);
            var element = document.RootElement;

            // Create a new JSON object with the required openapi version
            var modifiedJson = new Dictionary<string, JsonElement>();
            modifiedJson["openapi"] = JsonDocument.Parse("\"3.0.1\"").RootElement;

            // Copy all existing properties
            foreach (var property in element.EnumerateObject())
            {
                modifiedJson[property.Name] = property.Value;
            }

            context.Response.Body = originalBody;
            await JsonSerializer.SerializeAsync(context.Response.Body, modifiedJson);
        }
        else
        {
            await _next(context);
        }
    }
}