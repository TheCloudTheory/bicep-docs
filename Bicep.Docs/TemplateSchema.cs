using System.Text.Json.Serialization;

namespace Bicep.Docs;

internal class TemplateSchema
{
    [JsonPropertyName("parameters")]
    public IDictionary<string, TemplateParameter>? Parameters { get; set; }

    [JsonPropertyName("resources")]
    public Resource[]? Resources { get; set; }

    [JsonPropertyName("outputs")]
    public IDictionary<string, Output>? Outputs { get; set; }

    [JsonPropertyName("metadata")]
    public IDictionary<string, IDictionary<string, string>>? Metadata { get; set; }
}

internal class TemplateParameter
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("defaultValue")]
    public object? DefaultValue { get; set; }

    [JsonPropertyName("allowedValues")]
    public string[]? AllowedValues { get; set; }

    [JsonPropertyName("metadata")]
    public IDictionary<string, string>? Metadata { get; set; }
}

internal class Resource
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("apiVersion")]
    public string ApiVersion { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
}

internal class Output
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("value")]
    public string Value { get; set; } = null!;
}
