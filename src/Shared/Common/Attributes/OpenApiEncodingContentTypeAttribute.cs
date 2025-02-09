namespace Shared.Common.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class OpenApiEncodingContentTypeAttribute(string contentType) : Attribute
{
    public string ContentType { get; } = contentType;
}