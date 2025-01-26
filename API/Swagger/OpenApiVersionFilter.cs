using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Swagger;

public class OpenApiVersionFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Info.Version = "v1";
        swaggerDoc.Info.Title = "CSIC API";
        /*
        swaggerDoc.OpenApi = "3.0.1";
        */
        
    }
}