param parVnetName string
param parDnsZoneName string
param parDbName string
param parLocation string
param parAdminLogin string

@secure()
param parAdminPassword string

func GetSubnet(vnet object) string => vnet.properties.subnets[0].id

resource vnet 'Microsoft.Network/virtualNetworks@2023-09-01' existing = {
  name: parVnetName
}

resource dns 'Microsoft.Network/privateDnsZones@2020-06-01' existing = {
  name: parDnsZoneName
}

resource db 'Microsoft.DBforPostgreSQL/flexibleServers@2022-12-01' = {
  name: parDbName
  location: parLocation
  sku: {
    name: 'Standard_D4s_v3'
    tier: 'GeneralPurpose'
  }
  properties: {
    administratorLogin: parAdminLogin
    administratorLoginPassword: parAdminPassword
    version: '11'
    storage: {
      storageSizeGB: 128
    }
    network: {
      delegatedSubnetResourceId: GetSubnet(vnet)
      privateDnsZoneArmResourceId: dns.id
    }
  }
}
