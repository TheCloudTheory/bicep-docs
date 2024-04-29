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

    [Test]
    public void Basic_WhenBicepFileIsProvidedWithBooleanValueForParameter_DocumentationShouldBeGenerated()
    {
        Program.Main(["--template", "templates/keyvault/main.bicep"]);
    }

    [Test]
    public void Basic_WhenTemplateContainParameterTypeNumber_DocumentationShouldBeGenerated()
    {
        Program.Main(["--template", "templates/postgresql/main.bicep"]);
    }

    
    [Test]
    public void Basic_WhenTemplateResourcesArePresentedAsDictionary_DocumentationShouldBeGenerated()
    {
        Program.Main(["--template", "templates/basic/resourcesAsKeys.bicep"]);
    }
}