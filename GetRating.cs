using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;


namespace OrganicsThirdTry
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            var limit = 100;
            var limitQueryParameter = req.Query["limit"];

            var id = req.Query["id"];
            log.LogInformation(id);

            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };


            if (!string.IsNullOrWhiteSpace(limitQueryParameter))
                limit = int.Parse(limitQueryParameter);

            var collectionUri = UriFactory.CreateDocumentCollectionUri("ChallengeThree", "Ratings");

            var somethingDumb = System.Environment.GetEnvironmentVariable("CosmosEndpoint", EnvironmentVariableTarget.Process);
            log.LogInformation(somethingDumb);

            // var guid = new System.Guid("9337a7bc-176c-4971-bf74-41537bb78ba9");
            var guid = new System.Guid(id);

            var cosmosEndpointUri = new Uri(Environment.GetEnvironmentVariable("CosmosEndpoint", EnvironmentVariableTarget.Process));
            var cosmosKey = Environment.GetEnvironmentVariable("CosmosKey", EnvironmentVariableTarget.Process);

            DocumentClient dClient = new DocumentClient(cosmosEndpointUri, cosmosKey);


            var rating = dClient.CreateDocumentQuery<Rating>(collectionUri).Where(b => b.id == guid).AsEnumerable().FirstOrDefault();
            log.LogInformation(rating.ToString());


            // return new OkObjectResult(await query.ExecuteNextAsync());
            return new OkObjectResult(rating.ToString());
        }
    }

}
