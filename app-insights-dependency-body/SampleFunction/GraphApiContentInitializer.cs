using System.Net.Http;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace SampleFunction
{
    internal class GraphApiContentInitializer : ITelemetryInitializer
    {
        public const string HttpRequestOperationDetailName = "HttpRequest";
        public const string HttpResponseOperationDetailName = "HttpResponse";
        public const string HttpResponseHeadersOperationDetailName = "HttpResponseHeaders";

        
        public void Initialize(ITelemetry telemetry)
        {
            if (telemetry is DependencyTelemetry dependencyTelemetry)
            {
                if (!dependencyTelemetry.Data.Contains("graph.microsoft.com"))
                    return;

                AddRequestBody(dependencyTelemetry);
                AddResponseBody(dependencyTelemetry);
            }
        }

        private static void AddRequestBody(DependencyTelemetry dependencyTelemetry)
        {
            dependencyTelemetry.TryGetOperationDetail(HttpRequestOperationDetailName, out var result);

            if (result == null) return;

            if (result is HttpRequestMessage requestMessage)
            {
                if (requestMessage.Content == null) return;

                var content = requestMessage
                    .Content
                    .ReadAsStringAsync().Result;
                dependencyTelemetry.Properties.Add("request-body", content);
            }
        }

        private static void AddResponseBody(DependencyTelemetry dependencyTelemetry)
        {
            dependencyTelemetry.TryGetOperationDetail(HttpResponseOperationDetailName, out var result);

            if (result == null) return;

            if (result is HttpResponseMessage responseMessage)
            {
                if (responseMessage.Content == null) return;

                var content = responseMessage
                    .Content
                    .ReadAsStringAsync().Result;
                dependencyTelemetry.Properties.Add("response-body", content);
            }
        }
    }
}