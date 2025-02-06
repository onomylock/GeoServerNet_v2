using System.Reflection;
using Microsoft.OpenApi.Models;
using Shared.Common.Attributes;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shared.Common.Filters;

public sealed class MultipartRequestFilter<T> : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var schema = context.SchemaGenerator.GenerateSchema(typeof(T), context.SchemaRepository);
        var contentTypeByParameterName = context.MethodInfo.GetParameters()
            .Where(p => p.IsDefined(typeof(OpenApiEncodingContentTypeAttribute), true))
            .ToDictionary(p => p.Name, s => s.GetCustomAttribute<OpenApiEncodingContentTypeAttribute>()?.ContentType);


        operation.RequestBody = new OpenApiRequestBody
        {
            Content =
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = schema
                }
            }
        };

        if (contentTypeByParameterName.Any()) return;
        foreach (var requestContent in operation.RequestBody.Content)
        {
            var encodings = requestContent.Value.Encoding;
            foreach (var encoding in encodings)
                if (contentTypeByParameterName.TryGetValue(encoding.Key, out var value))
                    encoding.Value.ContentType = value;
        }
    }
}