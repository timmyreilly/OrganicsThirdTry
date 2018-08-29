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
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, 
            ILogger log,
            [CosmosDB(
                databaseName: "ChallengeTwo",
                collectionName: "Ratings",
                ConnectionStringSetting = "CosmosConnectionString",
                CreateIfNotExists = true)]IAsyncCollector<Rating> document
            )
        {
            try {
                log.LogInformation("C# Http trigger function processed a request... CreateRating");

                string requestBody = new StreamReader(req.Body).ReadToEnd(); 
                dynamic data = JsonConvert.DeserializeObject(requestBody); 

                // http://serverlessohuser.trafficmanager.net/api/GetUser
                // Validate User ID: 
                var response = await client.GetAsync($"http://serverlessohuser.trafficmanager.net/api/GetUser?={data.userId}")
                if (response.StatusCode != System.Net.HttpStatusCode.OK) {
                    return (ActionResult)new BadRequestObjectResult($"UserId: {data.userId} doesn't exist"); 
                } 

            } catch (Exception e){
                Console.WriteLine(e); 
                return (ActionResult)new BadRequestObjectResult("ERRRRORRRR"); 
            }
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
        public class Rating
        {
            public Guid id { get; set; }
            public Guid userId { get; set; }
            public Guid productId { get; set; }
            public DateTime timestamp { get; set; }
            public string locationName { get; set; }
            public int rating { get; set; }
            public string userNotes { get; set; }
            public int magicNumber { get; set; }

            public double sentimentScore { get; set; }
        }
    }
}
