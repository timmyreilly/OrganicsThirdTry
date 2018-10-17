
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.WebJobs.EventHubs;
// using Microsoft.Azure.EventGrid.Models;


namespace Company.Function
{
    public static class EventHubTrigger
    {
        [FunctionName("EventHubTrigger")]
        public static void Run(
            [EventHubTrigger("bighub", Connection = "EventHubsNameSpaceSASConnectionString")] string myEventHubMessage, TraceWriter log)
        {
            log.Info(myEventHubMessage);
        }
    }
}
