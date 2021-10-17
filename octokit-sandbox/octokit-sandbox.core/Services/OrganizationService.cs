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

        public async Task<List<string>> GetMembers(string org)
        {
            var client = _clientFactory.Create();
            var installation = await client.GitHubApps.GetOrganizationInstallationForCurrent(org);

            var orgClient = await _clientFactory.Create(installation.Id);
            var members = await orgClient.Organization.Member.GetAll(org);

            var result = members
                .Select(m => m.Login)
                .ToList();

            return result;
        }

        public async Task<Dictionary<int,string>> GetTeams(string org)
        {
            var client = _clientFactory.Create();
            var installation = await client.GitHubApps.GetOrganizationInstallationForCurrent(org);

            var orgClient = await _clientFactory.Create(installation.Id);
            var teams = await orgClient.Organization.Team.GetAll(org);
            

            var result = teams
                .ToDictionary(t => t.Id, t => t.Name);

            return result;
        }

        public async Task<List<string>> GetTeamMembers(string org, int teamId)
        {
            var client = _clientFactory.Create();
            var installation = await client.GitHubApps.GetOrganizationInstallationForCurrent(org);

            var orgClient = await _clientFactory.Create(installation.Id);
            var members = await orgClient.Organization.Team.GetAllMembers(teamId);

            var result = members
                .Select(m => m.Login)
                .ToList();

            return result;
        }
    }
}