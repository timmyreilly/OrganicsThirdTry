
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
// using Microsoft.Azure.EventGrid.Models;


namespace Company.Function
{
    public static class EventGridTrigger
    {
        [FunctionName("EventGridTrigger")]
        public static IActionResult Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var name = eventGridEvent.Data; 

            // var requestBody = new StreamReader(eventGridEvent).ReadToEnd();
            dynamic data = name; 
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
