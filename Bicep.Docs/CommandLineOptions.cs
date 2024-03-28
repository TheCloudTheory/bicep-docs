using CommandLine;

namespace Bicep.Docs;

internal class CommandLineOptions
{
    [Option('t', "template", Required = true, HelpText = "The path to the Bicep template file.")]
    public required string TemplatePath { get; set; }
}
