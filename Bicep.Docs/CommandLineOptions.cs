using CommandLine;

namespace Bicep.Docs;

internal class CommandLineOptions
{
    [Option('t', "template", Required = true, HelpText = "The path to the Bicep template file.")]
    public required string TemplatePath { get; set; }

    [Option('o', "output", Required = false, HelpText = "The path to the output file.", Default = "README.md")]
    public required string OutputPath { get; set; }
}
