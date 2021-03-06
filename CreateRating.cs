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
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents;


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
            TraceWriter log)
        {


            try
            {
                log.Info("C# Http trigger function processed a request... CreateRating");

                string requestBody = new StreamReader(req.Body).ReadToEnd();
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                // http://serverlessohuser.trafficmanager.net/api/GetUser pop
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


                var cosmosEndpointUri = new Uri(Environment.GetEnvironmentVariable("CosmosEndpoint", EnvironmentVariableTarget.Process));
                var cosmosKey = Environment.GetEnvironmentVariable("CosmosKey", EnvironmentVariableTarget.Process);

                using (var dClient = new DocumentClient(cosmosEndpointUri, cosmosKey))
                {

                    log.Info("GOT HERE");
                    dClient.CreateDatabaseIfNotExistsAsync(new Database() { Id = "ChallengeThree" }).GetAwaiter().GetResult();

                    dClient.CreateDocumentCollectionIfNotExistsAsync(
                    UriFactory.CreateDatabaseUri("ChallengeThree"),
                    new DocumentCollection { Id = "Ratings" }).
                    GetAwaiter()
                    .GetResult();

                    var rating = cosmosService.CreateRating(data);

                    dClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("ChallengeThree", "Ratings"), rating).GetAwaiter().GetResult();


                    return (ActionResult)new OkObjectResult(rating);
                }



            }
            catch (Exception e)
            {
                log.Info(e.ToString()); 
                return (ActionResult)new BadRequestObjectResult(e.ToString());
            }

        }

    }
}
