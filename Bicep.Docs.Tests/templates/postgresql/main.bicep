param parLocation string

@allowed([
  128
  256
  1024
])
param parStorageSize int = 1024

resource db 'Microsoft.DBforPostgreSQL/flexibleServers@2022-12-01' = {
  name: 'database'
  location: parLocation
  properties: {
    storage: {
      storageSizeGB: parStorageSize
    }
  }
}
