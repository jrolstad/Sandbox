using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octokit;
using octokit_sandbox.core.Factories;

namespace octokit_sandbox.core.Services
{
    public class OrganizationService
    {
        private readonly GitHubClientFactory _clientFactory;

        public OrganizationService(GitHubClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<List<string>> Get()
        {
            var client = _clientFactory.Create();

            var installations = await client.GitHubApps.GetAllInstallationsForCurrent();

            var installedOrganizations = installations
                .Where(i => i.TargetType.Value == AccountType.Organization)
                .Select(i => i.Account.Login)
                .ToList();

            return installedOrganizations;
        }
    }
}