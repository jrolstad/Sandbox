using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConditionalAccessLoadTest.Factories
{
    public class GraphClientFactory
    {
        private readonly string _tenantId;
        private readonly string _clientId;
        private readonly string _clientSecret;

        public GraphClientFactory(string tenantId, 
            string clientId, 
            string clientSecret)
        {
            _tenantId = tenantId;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        private GraphServiceClient _instance;

        public GraphServiceClient Create()
        {
            if (_instance != null) return _instance;
           
            var graphServiceClient = new GraphServiceClient("https://graph.microsoft.com/beta/",
               new DelegateAuthenticationProvider(async requestMessage =>
               {
                   var token = await GetAccessToken(_tenantId,_clientId,_clientSecret, "https://graph.microsoft.com/.default");
                   requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
               }));

            return graphServiceClient;
        }

        private static async Task<string> GetAccessToken(string tenantId, string clientId, string clientSecret, params string[] scopes)
        {
            var app = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithTenantId(tenantId)
                .WithClientSecret(clientSecret)
                .Build();

            var result = await app.AcquireTokenForClient(scopes)
                .ExecuteAsync();

            return result.AccessToken;
        }
    }
}
