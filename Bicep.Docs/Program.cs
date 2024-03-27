﻿using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bicep.Docs;

public class Program
{
    public static void Main(string[] args)
    {
        var templatePath = args[0];

        if (File.Exists(templatePath))
        {
            var file = new FileInfo(templatePath);
            var rawTemplate = BicepCompiler.Compile(file, CancellationToken.None);

#if DEBUG
            Console.WriteLine(rawTemplate);
#endif

            var parameters = JsonSerializer.Deserialize<TemplateSchema>(rawTemplate!);

            var sb = new StringBuilder();
            sb.AppendLine($"# {file.Name}");

            if (parameters?.Parameters != null)
            {
                sb.AppendLine("## Parameters");
                sb.AppendLine("| Name | Description | Type | Default value | Required? | Allowed values |");
                sb.AppendLine("|------|-------------|------|---------------|-----------|----------------|");

                foreach (var (name, parameter) in parameters.Parameters)
                {
                    var defaultValue = parameter.DefaultValue != null ? $"`{parameter.DefaultValue}`" : "N/A";
                    var isRequired = defaultValue == "N/A" ? "Yes" : "No";
                    var allowedValues = parameter.AllowedValues != null ? $"`{string.Join($" / ", parameter.AllowedValues)}`" : "N/A";
                    var description = parameter.Metadata?["description"] ?? "N/A";

                    sb.AppendLine($"| {name} | {description} | {parameter.Type} | {defaultValue} | {isRequired} | {allowedValues} |");
                }
            }

            if (parameters?.Resources != null)
            {
                sb.AppendLine("## Resources");
                sb.AppendLine("| Type | API Version | Name |");
                sb.AppendLine("|------|-------------|------|");

                foreach (var resource in parameters.Resources)
                {
                    sb.AppendLine($"| {resource.Type} | {resource.ApiVersion} | `{resource.Name}` |");
                }
            }

            if (parameters?.Outputs != null)
            {
                sb.AppendLine("## Outputs");
                sb.AppendLine("| Name | Type | Value |");
                sb.AppendLine("|------|------|-------|");

                foreach (var (name, output) in parameters.Outputs)
                {
                    sb.AppendLine($"| {name} | {output.Type} | `{output.Value}` |");
                }
            }

            var result = sb.ToString();
            File.WriteAllText("documentation.md", result);
        }
        else
        {
            throw new FileNotFoundException($"File not found: {templatePath}");
        }
    }
}

internal class TemplateSchema
{
    [JsonPropertyName("parameters")]
    public IDictionary<string, TemplateParameter>? Parameters { get; set; }

    [JsonPropertyName("resources")]
    public Resource[]? Resources { get; set; }

    [JsonPropertyName("outputs")]
    public IDictionary<string, Output>? Outputs { get; set; }
}

internal class TemplateParameter
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("defaultValue")]
    public string? DefaultValue { get; set; }

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