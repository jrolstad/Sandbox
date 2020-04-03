using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SampleFunction.Models.Graph;

namespace SampleFunction
{
    public static class Functions
    {
        [FunctionName("samples-response-body")]
        public static async Task TrackResponseBody([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, 
            ILogger log)
        {
            var accessToken = await GetAccessToken();

            var client = HttpClientFactory.Create();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",accessToken);

            var result = await client.GetAsync("https://graph.microsoft.com/beta/me/");
            result.EnsureSuccessStatusCode();
        }

        [FunctionName("samples-request-body")]
        public static async Task TrackRequestBody([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer,
            ILogger log)
        {
            var accessToken = await GetAccessToken();

            var client = HttpClientFactory.Create();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var request = new TaggableItem
            {
                id = "0ff3ff8a-ff4a-45ef-9235-479409b5552e",
                tags = new List<string> {$"now-is-the-time|{DateTime.Now}"}
            };
            var url = $"https://graph.microsoft.com/beta/applications/{request.id}";

            var content = new ObjectContent<TaggableItem>(request, new JsonMediaTypeFormatter());
            var result = await client.PatchAsync(url, content);
            result.EnsureSuccessStatusCode();

        }

        private static async Task<string> GetAccessToken()
        {
            var tokenService = new AzureServiceTokenProvider();
            var token = await tokenService.GetAccessTokenAsync("https://graph.microsoft.com/");

            return token;
        }


    }
}
