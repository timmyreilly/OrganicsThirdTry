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
    public static class GetRatings
    {
        [FunctionName("GetRatings")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ratings")]HttpRequest req,
            [CosmosDB("ChallengeTwo", "Ratings",
                ConnectionStringSetting = "CosmosConnectionString")]
                DocumentClient documentClient,
            TraceWriter log
        )
        {
            var limit = 100;
            var limitQueryParameter = req.Query["limit"];

            if (!string.IsNullOrWhiteSpace(limitQueryParameter))
                limit = int.Parse(limitQueryParameter);
            
            var collectionUri = UriFactory.CreateDocumentCollectionUri("ChallengeTwo", "Ratings");

            IDocumentQuery<Rating> query = documentClient.CreateDocumentQuery<Rating>(collectionUri)
                .Take(limit)
                .AsDocumentQuery();

            return new OkObjectResult(await query.ExecuteNextAsync());
        }
    }
}
