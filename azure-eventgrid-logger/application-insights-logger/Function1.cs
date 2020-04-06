// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace application_insights_logger
{
    public static class Function1
    {
        [FunctionName("eventgrid-listener")]
        public static void Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            var data = JsonConvert.SerializeObject(eventGridEvent);
            log.LogInformation($"Message Received from Event Grid|{data}");
        }
    }
}
