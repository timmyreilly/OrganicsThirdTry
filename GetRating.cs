using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Threading.Tasks;

namespace OrganicsThirdTry
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "rating")]HttpRequest req,
            [CosmosDB("ChallengeTwo", "Ratings",
                ConnectionStringSetting = "CosmosConnectionString")]
                DocumentClient documentClient,
            TraceWriter log
        )
        {
            var limit = 100;
            var limitQueryParameter = req.Query["limit"];

            var id = req.Query["id"];
            log.Info(id);

            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            // IQueryable<Rating> ratingQuery = documentClient.CreateDocumentQuery<Rating>(
            //     UriFactory.CreateDocumentCollectionUri("ChallengeTwo", "Ratings"), queryOptions).Where(i => i.id == id); 
            // ) 


            if (!string.IsNullOrWhiteSpace(limitQueryParameter))
                limit = int.Parse(limitQueryParameter);

            var collectionUri = UriFactory.CreateDocumentCollectionUri("ChallengeTwo", "Ratings");
            // IDocumentQuery<dynamic> q = documentClient.CreateDatabaseQuery<Rating>("Select * from Ratings r where r.id = id").AsDocumentQuery(); 

            IDocumentQuery<Rating> rdog = documentClient.CreateDocumentQuery<Rating>(collectionUri).Where(rt => rt.id == id).AsDocumentQuery();
            // Assert("Expected only 1 rating", rdog.ToList().Count == 1); 

            var guid = new System.Guid("9337a7bc-176c-4971-bf74-41537bb78ba9");

            var rating = documentClient.CreateDocumentQuery<Rating>(collectionUri).Where(b => b.id == guid ).AsEnumerable().FirstOrDefault(); 
            log.Info(rating.ToString()); 

            IDocumentQuery<Rating> query = documentClient.CreateDocumentQuery<Rating>(collectionUri)
                .Take(limit)
                .AsDocumentQuery();

            // return new OkObjectResult(await query.ExecuteNextAsync());
            return new OkObjectResult(rating.ToString());
        }

        //     [FunctionName("CosmosDBSample")]
        //     public static HttpResponseMessage Run(
        // [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "foo/{id}")] HttpRequestMessage req,
        // [CosmosDB(databaseName: "ChallengeTwo",
        //             collectionName: "Ratings",
        //             ConnectionStringSetting = "CosmosConnectionString", Id = "{id}")] object document)
        //     {
        //         if (document == null)
        //         {
        //             return req.CreateResponse(System.Net.HttpStatusCode.NotFound);
        //         }
        //         else
        //         {
        //             return req.CreateResponse(System.Net.HttpStatusCode.OK, document);
        //         }
        //     }

        //     [FunctionName("COSMSOSOSOSOSOSOS")]
        //     public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "foo/{id}")] HttpRequestMessage req,
        // [CosmosDB(databaseName: "ChallengeTwo",
        //             collectionName: "Ratings",
        //             ConnectionStringSetting = "CosmosConnectionString", Id = "{id}")] object document, TraceWriter log)
        //     {
        //         if (document == null)
        //         {
        //             return req.CreateResponse(System.Net.HttpStatusCode.NotFound);
        //         }
        //         else
        //         {
        //             return req.CreateResponse(System.Net.HttpStatusCode.OK, document);
        //         }
        //     }
    }

}
