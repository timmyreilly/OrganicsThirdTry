
To add settings for Cosmos: 
`func settings add CosmosConnectionString "AccountEndpoint=https://asldkfjasldkfjasdf CONNECTION STRING"`

To Install Extensions: 
`Microsoft.Azure.WebJobs.Extensions.EventGrid -Version 1.0.0` 

Or Manually Add it to .csproj: 
`<PackageReference Include="Microsoft.Azure.WebJobs.Extensions.EventGrid" Version="1.0.0" />`

// Some Quick Notes...

Make sure you convert the Id coming in to a Guid. 

Can't deploy dotnet core to Linux

CosmosConnectionString is a application settings... Not a connection string. 



Had to add this to Csproj: 
```XML 
    <None Update="proxies.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>

    </None>
``` 

Like this: 

```XML
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
``` 

### Some Endpoints: 

#### Given Hosted API: 
- Get Products: ` http://serverlessohproduct.trafficmanager.net/api/GetProducts `
- Get Product: ` http://serverlessohproduct.trafficmanager.net/api/GetProduct ` 
- Get User: ` http://serverlessohuser.trafficmanager.net/api/GetUser ` 


#### Azure Function: 
- Get Specific Rating: ` https://dumbdumbthree.azurewebsites.net/api/GetRating?id=f7661784-2c69-4f6c-bfd3-1f4cdafe2087 `
- Get All Ratings: ` https://dumbdumbthree.azurewebsites.net/api/ratings `
- Get Product (Proxy): ` https://dumbdumbthree.azurewebsites.net/api/GetProduct?productId=75542e38-563f-436f-adeb-f426f1dabb5c `
- Get User (Proxy): ` https://dumbdumbthree.azurewebsites.net/api/GetUser?userId=cc20a6fb-a91f-4192-874d-132493685376 `
- Get All Products (Proxy): ` https://dumbdumbthree.azurewebsites.net/api/GetPRoducts ` 
- Create Rating: 
    - URL: `https://dumbdumbthree.azurewebsites.net/api/CreateRating`
    - BODY:  
    ```JSON 
    {
        "userId": "cc20a6fb-a91f-4192-874d-132493685376",
        "productId": "4c25613a-a3c2-4ef3-8e02-9c335eb23204",
        "locationName": "Sample ice cream shop",
        "rating": 5,
        "userNotes": "I love the subtle notes of cheese in this poo burger!"
    }
    ``` 