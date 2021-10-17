using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using octokit_sandbox.core.Configuration;
using octokit_sandbox.core.Factories;
using octokit_sandbox.core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        [Fact]
        public async Task Get_NoInputs_GetsAllInstalledOrganizations()
        {
            var provider = ConfigureApplication();

            var service = provider.GetService<OrganizationService>();

            var organizations = await service.Get();

            Assert.NotEmpty(organizations);
        }

        [Fact]
        public async Task GetMembers_ValidOrganization_GetsAllMembers()
        {
            var provider = ConfigureApplication();

            var service = provider.GetService<OrganizationService>();

            var organizations = await service.Get();

            foreach (var item in organizations)
            {
                var members = await service.GetMembers(item);
                Assert.NotEmpty(members);
            }
        }

        [Fact]
        public async Task GetTeams_ValidOrganization_GetsAllTeams()
        {
            var provider = ConfigureApplication();

            var service = provider.GetService<OrganizationService>();

            var organizations = await service.Get();

            foreach (var item in organizations)
            {
                var teams = await service.GetTeams(item);
                Assert.NotEmpty(teams);
            }
        }

        [Fact]
        public async Task GetTeamMembers_ValidTeam_GetsAllMembers()
        {
            var provider = ConfigureApplication();

            var service = provider.GetService<OrganizationService>();

            var organizations = await service.Get();

            foreach (var item in organizations)
            {
                var teams = await service.GetTeams(item);
                foreach (var team in teams)
                {
                    var members = await service.GetTeamMembers(item, team.Key);
                    Assert.NotEmpty(members);
                }
                
            }
        }

        [Fact]
        public async Task GetTeamMemberRoles_ValidTeam_GetsAllMembers()
        {
            var provider = ConfigureApplication();

            var service = provider.GetService<OrganizationService>();

            var organizations = await service.Get();

            foreach (var item in organizations)
            {
                var teams = await service.GetTeams(item);
                foreach (var team in teams)
                {
                    var members = await service.GetTeamMemberRoles(item, team.Key);
                    Assert.NotEmpty(members);
                }

            }
        }

        [Fact]
        public async Task GetMemberRoles_ValidOrganization_GetsRolesOfAllMembers()
        {
            var provider = ConfigureApplication();

            var service = provider.GetService<OrganizationService>();

            var organizations = await service.Get();

            foreach (var item in organizations)
            {
                var roles = await service.GetMemberRoles(item);
                Assert.NotEmpty(roles);
            }
        }

        [Fact]
        public async Task GetRepositories_ValidOrganization_GetsAllRepositories()
        {
            var provider = ConfigureApplication();

            var service = provider.GetService<OrganizationService>();

            var organizations = await service.Get();

            foreach (var item in organizations)
            {
                var repositories = await service.GetRepositories(item);
                Assert.NotEmpty(repositories);
            }
        }

        [Fact]
        public async Task GetRepositoryCollaborators_ValidOrganizationAndRepository_GetsAllCollaborators()
        {
            var provider = ConfigureApplication();

            var service = provider.GetService<OrganizationService>();

            var organizations = await service.Get();

            foreach (var item in organizations)
            {
                var repositories = await service.GetRepositories(item);

                foreach (var repo in repositories)
                {
                    var collaborators = await service.GetRepositoryCollaborators(item,repo.Key);

                    Assert.NotEmpty(collaborators);
                }
            }
        }

        [Fact]
        public async Task GetRepositoryCollaboratorPermissions_ValidOrganizationAndRepository_GetsAllCollaborators()
        {
            var provider = ConfigureApplication();

            var service = provider.GetService<OrganizationService>();

            var organizations = await service.Get();

            foreach (var item in organizations)
            {
                var repositories = await service.GetRepositories(item);

                foreach (var repo in repositories)
                {
                    var collaborators = await service.GetRepositoryCollaboratorPermissions(item, repo.Key);

                    Assert.NotEmpty(collaborators);
                }
            }
        }

        [Fact]
        public async Task GetRepositoriesForUser_ValidOrganization_GetsAllRepositoriesUserIsAuthorizedFor()
        {
            var provider = ConfigureApplication();

            var service = provider.GetService<OrganizationService>();

            var organizations = await service.Get();

            foreach (var item in organizations)
            {
                var repositories = await service.GetRepositoriesForUser(item,"oppknox");
                Assert.NotEmpty(repositories);
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
