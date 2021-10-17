using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using octokit_sandbox.core.Configuration;
using octokit_sandbox.core.Factories;
using Xunit;

namespace octokit_sandbox.tests
{
    public class UnitTest1
    {
        [Fact]
        public void Create_NoInputs_GetsClient()
        {
            var provider = ConfigureApplication();

            var factory = provider.GetService<GitHubClientFactory>();

            var client = factory.Create();

            Assert.NotNull(client);
        }

        [Fact]
        public async Task Create_NoInputs_GetsAllInstallations()
        {
            var provider = ConfigureApplication();
            var factory = provider.GetService<GitHubClientFactory>();
            var client = factory.Create();

            var result = await client.GitHubApps.GetAllInstallationsForCurrent();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Create_NoInputs_GetsInstallationDetails()
        {
            var provider = ConfigureApplication();
            var factory = provider.GetService<GitHubClientFactory>();
            var client = factory.Create();

            var installations = await client.GitHubApps.GetAllInstallationsForCurrent();

            foreach (var item in installations)
            {

                var installationClient = await factory.Create(item.Id);

                if (item.TargetType.Value == AccountType.Organization)
                {
                    var organization = await installationClient.Organization.Get(item.Account.Login);

                    Assert.NotNull(organization);
                }
                else if (item.TargetType.Value == AccountType.User)
                {
                    var user = await installationClient.User.Get(item.Account.Login);

                    Assert.NotNull(user);
                }
                else
                {
                    throw new Exception("Unrecognized type");
                }


            }
        }

        private static ServiceProvider ConfigureApplication()
        {
            var configurations = new Dictionary<string, string>
            {
                {ConfigurationNames.GitHubApp.Id, "145448"},
                {ConfigurationNames.KeyVault.BaseUri, "https://msft-sandbox.vault.azure.net/"},
                {ConfigurationNames.KeyVault.ManagedIdentityClient, ""},
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configurations)
                .Build();

            var services = new ServiceCollection();

            DependencyInjectionConfiguration.Configure(services, configuration);

            var provider = services.BuildServiceProvider();
            return provider;
        }
    }
}
