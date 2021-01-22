using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Pipeline;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using fiveFunction.Services;

namespace fiveFunction
{
    public class Function1
    {
        private readonly IHttpResponderService _httpResponderService;

        public Function1(IHttpResponderService httpResponderService)
        {
            _httpResponderService = httpResponderService;
        }

        [FunctionName("Function1")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req,
                    FunctionExecutionContext executionContext)
        {
            var logger = executionContext.Logger;
            logger.LogInformation("message logged");

            var response = _httpResponderService.ProcessRequest(req);

            return response;
        }

    }
}