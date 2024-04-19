@description('Create a Key Vault with the specified location')
@allowed([ 'eastus', 'westus', 'centralus' ])
param parLocation string = 'eastus'

@description('Enable the Key Vault for deployment')
param parEnabledForDeployment bool = true

resource kv 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: 'kv'
  location: parLocation
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: tenant().tenantId
    enabledForDeployment: parEnabledForDeployment
  }
}
