using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace OrganicsThirdTry
{
    /*
    Request:
    {
        "userId": "cc20a6fb-a91f-4192-874d-132493685376",
        "productId": "4c25613a-a3c2-4ef3-8e02-9c335eb23204",
        "locationName": "Sample ice cream shop",
        "rating": 5,
        "userNotes": "I love the subtle notes of orange in this ice cream!"
    }
    Response:
    {
        "id": "79c2779e-dd2e-43e8-803d-ecbebed8972c",
        "userId": "cc20a6fb-a91f-4192-874d-132493685376",
        "productId": "4c25613a-a3c2-4ef3-8e02-9c335eb23204",
        "timestamp": "2018-05-21 21:27:47Z",
        "locationName": "Sample ice cream shop",
        "rating": 5,
        "userNotes": "I love the subtle notes of orange in this ice cream!"
    }
    */
    public static class CreateRating
    {
        private static readonly HttpClient client = new HttpClient();

        [FunctionName("CreateRating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req,
            ILogger log,
            [CosmosDB(
                databaseName: "ChallengeTwo",
                collectionName: "Ratings",
                ConnectionStringSetting = "CosmosConnectionString",
                CreateIfNotExists = true)]IAsyncCollector<Rating> document
            )
        {
            try
            {
                log.LogInformation("C# Http trigger function processed a request... CreateRating");

                string requestBody = new StreamReader(req.Body).ReadToEnd();
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                // http://serverlessohuser.trafficmanager.net/api/GetUser
                // Validate User ID: 
                var response = await client.GetAsync($"http://serverlessohuser.trafficmanager.net/api/GetUser?userId={data.userId}");
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return (ActionResult)new BadRequestObjectResult($"UserId: {data.userId} doesn't exist");
                }

                // http://serverlessohproduct.trafficmanager.net/api/GetProduct 
                // Validate productId:
                response = await client.GetAsync($"http://serverlessohproduct.trafficmanager.net/api/GetProduct?productId={data.productId}");
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return (ActionResult)new BadRequestObjectResult($"ProductId: {data.productId} doesn't exist");
                }

                int ratingValue = -1;
                if (!int.TryParse((string)data.rating, out ratingValue) || ratingValue < 0 || ratingValue > 5)
                {
                    return (ActionResult)new BadRequestObjectResult("Rating needs to be between 0 and 5");
                }

                var cosmosService = new CosmosService();
                var rating = await cosmosService.CreateRatingFromDocument(data, document, client, log);

                return (ActionResult)new OkObjectResult(rating);


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (ActionResult)new BadRequestObjectResult("ERRRRORRRR");
            }

        }

    }
}
