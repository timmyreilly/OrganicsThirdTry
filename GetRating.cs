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
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ratings/{id}")]HttpRequestMessage req,
            [CosmosDB(
                databaseName: "Challenge2",
                collectionName: "Ratings",
                ConnectionStringSetting = "CosmosConnectionString",
                Id = "{id}")]
                //SqlQuery = "select * from Ratings r where r.id = {id}")]
                OrganicsThirdTry.Rating document, TraceWriter log)
        {
            if (document == null)
            {
                log.Info("Not found"); 
                log.Info(document.ToString()); 
                return req.CreateResponse(System.Net.HttpStatusCode.NotFound);
            }
            else
            {
                return req.CreateResponse(System.Net.HttpStatusCode.OK, document);
            }
        }
    }
}
