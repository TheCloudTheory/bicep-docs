param parLocation string = 'polandcentral'

module basic '../basic.bicep' = {
  name: 'basic'
  params: {
    parAcrName: 'bicepdocsacr' 
    parSqlAdminLogin: 'bicepdocsadmin'
    parSqlName: 'bicepdocssql'
    parLocation: parLocation
  }
}
