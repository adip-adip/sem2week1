using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace sem2week1.Swagger;

public class FileUploadOperationFilter : IParameterFilter
{
    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        if (context.ApiParameterDescription?.ParameterDescriptor?.ParameterType == typeof(IFormFile) ||
            context.ApiParameterDescription?.ParameterDescriptor?.ParameterType == typeof(IEnumerable<IFormFile>))
        {
            parameter.Schema = new OpenApiSchema
            {
                Type = "string",
                Format = "binary"
            };
        }
    }
}