using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Octokit;
using octokit_sandbox.core.Configuration;
using static System.Int32;

namespace octokit_sandbox.core.Factories
{
    public class GitHubClientFactory
    {
        private readonly SecretClient _secretClient;
        private readonly IConfiguration _configuration;

        public GitHubClientFactory(SecretClient secretClient, IConfiguration configuration)
        {
            _secretClient = secretClient;
            _configuration = configuration;
        }

        public GitHubClient Create()
        {
            var token = GetJwtToken();
            var appClient = new GitHubClient(new ProductHeaderValue("IdentityGuard"))
            {
                Credentials = new Credentials(token, AuthenticationType.Bearer)
            };

            return appClient;
        }

        public async Task<GitHubClient> Create(long installationId)
        {
            var appClient = Create();
            var response = await appClient.GitHubApps.CreateInstallationToken(installationId);

            var client = new GitHubClient(new ProductHeaderValue("IdentityGuard"))
            {
                Credentials = new Credentials(response.Token)
            };

            return client;
        }

        private string GetJwtToken()
        {
            var appIdValue = _configuration[ConfigurationNames.GitHubApp.Id];
            var appId = Parse(appIdValue);

            var privateKey = _secretClient.GetSecret(SecretNames.GitHubAppSecret).Value.Value;

            var generator = new GitHubJwt.GitHubJwtFactory(
                new GitHubJwt.StringPrivateKeySource(privateKey), 
                new GitHubJwt.GitHubJwtFactoryOptions
                {
                    AppIntegrationId = appId, // The GitHub App Id
                    ExpirationSeconds = 500 // 10 minutes is the maximum time allowed
                }
            );

            return generator.CreateEncodedJwtToken();
        }
    }
}