using System;
using System.Collections.Generic;
using System.Text;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using octokit_sandbox.core.Factories;
using octokit_sandbox.core.Services;

namespace octokit_sandbox.core.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient((provider) => configuration);

            services.AddTransient<GitHubClientFactory>();
            services.AddTransient<OrganizationService>();

            ConfigureKeyVault(services);
        }

        private static void ConfigureKeyVault(IServiceCollection services)
        {
            services.AddScoped(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                var endpoint = configuration[ConfigurationNames.KeyVault.BaseUri];
                var endpointUrl = new Uri(endpoint);
                var managedIdentityClientId = configuration[ConfigurationNames.KeyVault.ManagedIdentityClient];

                var credentials = GetCredential(managedIdentityClientId);

                return new SecretClient(vaultUri: endpointUrl, credential: credentials);
            });

        }

        private static TokenCredential GetCredential(string clientId)
        {
            if (string.IsNullOrWhiteSpace(clientId)) return new DefaultAzureCredential();

            return new ManagedIdentityCredential(clientId);
        }
    }
}
