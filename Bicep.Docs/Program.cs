using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommandLine;

namespace Bicep.Docs;

public class Program
{
    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<CommandLineOptions>(args)
            .WithParsed(o =>
            {
                if (File.Exists(o.TemplatePath))
                {
                    var file = new FileInfo(o.TemplatePath);
                    var rawTemplate = file.Extension == ".json" ? File.ReadAllText(o.TemplatePath) : BicepCompiler.Compile(file, CancellationToken.None);

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

                    if(parameters?.Metadata != null)
                    {
                        if(parameters.Metadata.TryGetValue("BicepDocs", out IDictionary<string, string>? docsMetadata))
                        {
                            if(docsMetadata.TryGetValue("examplesDirectory", out string? examplesDirectory))
                            {
                                if(Directory.Exists(examplesDirectory) == false)
                                {
                                    throw new DirectoryNotFoundException($"Directory not found: {examplesDirectory}");
                                }

                                sb.AppendLine("## Examples");
                               
                                var examples = Directory.GetFiles(examplesDirectory, "*.bicep", SearchOption.AllDirectories);
                                foreach (var example in examples)
                                {
                                    var exampleFile = new FileInfo(example);
                                    var exampleName = exampleFile.Name.Replace(exampleFile.Extension, string.Empty);
                                    var exampleContent = File.ReadAllText(example);

                                    sb.AppendLine($"### {exampleName}");
                                    sb.AppendLine("```bicep");
                                    sb.AppendLine(exampleContent);
                                    sb.AppendLine("```");
                                }
                            }
                        }   
                    }

                    var result = sb.ToString();
                    File.WriteAllText("documentation.md", result);
                }
                else
                {
                    throw new FileNotFoundException($"File not found: {o.TemplatePath}");
                }
            });
    }
}