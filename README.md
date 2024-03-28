# bicep-docs
Generate Bicep documentation with a single command

## Usage
```
./Bicep.Docs --template templates/main.bicep
```

## Prerequisites
You need to have Azure CLI or Bicep CLI installed prior to run Bicep.Docs.

## Installation
Bicep.Docs is published as a self-contained package for Windows, Linux and OSX. Check the Releases page for appropriate package. No installation needed.

## Example
Let's assume your Bicep file (`basic.bicep`) looks like this:
```
@description('Name of the Azure Container Registry')
param parAcrName string

@description('Location of the Azure Container Registry')
param parLocation string = resourceGroup().location

@allowed([
  'Basic'
  'Standard'
  'Premium'
])
@description('The SKU of the Azure Container Registry')
param parSku string = 'Basic'

@description('Name of the Azure SQL Server')
param parSqlName string

@secure()
@description('Password for the Azure SQL Server admin user')
param parSqlAdminLogin string

resource acr 'Microsoft.ContainerRegistry/registries@2023-07-01' = {
  name: parAcrName
  location: parLocation
  sku: {
    name: parSku
  }
  properties: {
    adminUserEnabled: false
  }
}

resource sql 'Microsoft.Sql/servers@2021-11-01' = {
  name: parSqlName
  location: parLocation
  properties: {
    administratorLogin: 'admin'
    administratorLoginPassword: parSqlAdminLogin
  }
}

output acrId string = acr.id
output sqlId string = sql.id
```
After running Bicep.Docs with the following command:
```
./Bicep.Docs "basic.bicep"
```
The generated documentation file (`documentation.md`) will look like so:
```
# basic.bicep
## Parameters
| Name | Description | Type | Default value | Required? | Allowed values |
|------|-------------|------|---------------|-----------|----------------|
| parAcrName | Name of the Azure Container Registry | string | N/A | Yes | N/A |
| parLocation | Location of the Azure Container Registry | string | `[resourceGroup().location]` | No | N/A |
| parSku | The SKU of the Azure Container Registry | string | `Basic` | No | `Basic / Standard / Premium` |
| parSqlName | Name of the Azure SQL Server | string | N/A | Yes | N/A |
| parSqlAdminLogin | Password for the Azure SQL Server admin user | securestring | N/A | Yes | N/A |
## Resources
| Type | API Version | Name |
|------|-------------|------|
| Microsoft.ContainerRegistry/registries | 2023-07-01 | `[parameters('parAcrName')]` |
| Microsoft.Sql/servers | 2021-11-01 | `[parameters('parSqlName')]` |
## Outputs
| Name | Type | Value |
|------|------|-------|
| acrId | string | `[resourceId('Microsoft.ContainerRegistry/registries', parameters('parAcrName'))]` |
| sqlId | string | `[resourceId('Microsoft.Sql/servers', parameters('parSqlName'))]` |

```

## Adding templates
If you want, you may automatically add templates for your Bicep files, which will be incorporated into the documentation file. All you need is to provide the path to the examples using `metadata` object:
```
metadata BicepDocs = {
  examplesDirectory: 'templates/examples'
}
```
For instance, let's assume the following directory structure:
```
templates
\ -- basic.bicep
\ -- examples
     \ -- example1.bicep
```
Now, if you run the following command:
```
./Bicep.Docs --template templates/basic.bicep
```
`Bicep.Docs` will automatically read the content of `example1.bicep` and put it in the generated documentation file.
