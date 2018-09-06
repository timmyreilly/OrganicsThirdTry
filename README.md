
To add settings for Cosmos: 
`func settings add CosmosConnectionString "AccountEndpoint=https://asldkfjasldkfjasdf CONNECTION STRING"`

// Some Quick Notes...

Make sure you convert the Id coming in to a Guid. 

Can't deploy dotnet core to Linux

CosmosConnectionString is a application settings... Not a connection string. 

Get Products: http://serverlessohproduct.trafficmanager.net/api/GetProducts
Get Product: http://serverlessohproduct.trafficmanager.net/api/GetProduct 
Get User: http://serverlessohuser.trafficmanager.net/api/GetUser 


Had to add this to Csproj: 
'''
    <None Update="proxies.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>

    </None>
'''

Like this: 

'''
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>netstandard2.0</TargetFramework>
    <AzureFunctionsVersion>v2</AzureFunctionsVersion>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.Azure.WebJobs.Extensions.CosmosDB" Version="3.0.0-beta7" />
    <PackageReference Include="Microsoft.NET.Sdk.Functions" Version="1.0.14" />
  </ItemGroup>
  <ItemGroup>
    <None Update="host.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Update="proxies.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>

    </None>
    <None Update="local.settings.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
      <CopyToPublishDirectory>Never</CopyToPublishDirectory>
    </None>
  </ItemGroup>
</Project>
'''