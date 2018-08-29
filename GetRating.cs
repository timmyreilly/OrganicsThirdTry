
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace OrganicsThirdTry
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ratings/{id:guid}")]HttpRequest req,
             [CosmosDB(
                databaseName: "Challenge2",
                collectionName: "Ratings",
                ConnectionStringSetting = "CosmosConnectionString",
                PartitionKey = "{id}",
                Id = "{id}")]
                //SqlQuery = "select * from Ratings r where r.id = {id}")]
                Rating rating, TraceWriter log)
        {
            return (ActionResult)new OkObjectResult(rating);
        }
    }
}
