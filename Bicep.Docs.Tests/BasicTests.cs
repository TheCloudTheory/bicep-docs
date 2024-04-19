namespace Bicep.Docs.Tests;

public class BasicTests
{
    [Test]
    public void Basic_WhenBicepFileIsProvided_DocumentationShouldBeGenerated()
    {
        Program.Main(["--template", "templates/basic/basic.bicep"]);
    }

    [Test]
    public void Basic_WhenJsonFileIsProvided_DocumentationShouldBeGenerated()
    {
        Program.Main(["--template", "templates/json/basic.json"]);
    }
}