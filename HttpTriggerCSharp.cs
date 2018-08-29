
using System.IO;
using System; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.CosmosDB; // install like this: > func extensions install --package Microsoft.Azure.WebJobs.Extensions.CosmosDB --version 3.0.0-beta7

namespace Company.Function
{
    public static class HttpTriggerCSharp
    {
        [FunctionName("HttpTriggerCSharp")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, 
            ILogger log,
            [CosmosDB(
                databaseName: "DatabaseThree",
                collectionName: "collectionOne",
                ConnectionStringSetting = "CosmosConnectionString",
                CreateIfNotExists = true 

            )]out dynamic document)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            // log.LogInformation("Something you might be interested in: " + ConfigurationManager.AppSettings["cosmosConnectionString"]);
            // log.LogInformation("Something you might be interested in: " + ConfigurationManager.AppSettings["cosmosStringFromHost"]);

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            document = new { Description = name, IDesignTimeMvcBuilderConfiguration= Guid.NewGuid()}; 


             return document != null
                 ? (ActionResult)new OkObjectResult($"Hello, {name}")
                 : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
