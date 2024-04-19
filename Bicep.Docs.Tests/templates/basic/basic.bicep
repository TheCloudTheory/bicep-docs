metadata BicepDocs = {
  examplesDirectory: 'templates/basic/examples'
}

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
