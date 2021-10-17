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

        public async Task<Dictionary<string,string>> GetTeamMemberRoles(string org, int teamId)
        {
            var client = _clientFactory.Create();
            var installation = await client.GitHubApps.GetOrganizationInstallationForCurrent(org);

            var orgClient = await _clientFactory.Create(installation.Id);
            var members = await orgClient.Organization.Team.GetAllMembers(teamId);

            var roleTasks = members
                .Select(m => GetTeamMemberRole(orgClient,teamId,m.Login));

            var roles = await Task.WhenAll(roleTasks);

            var results = roles
                .ToDictionary(r => r.Key, r => r.Value);

            return results;
        }

        private async Task<KeyValuePair<string, string>> GetTeamMemberRole(GitHubClient client, int teamId, string login)
        {
            var role = await client.Organization.Team.GetMembershipDetails(teamId, login);

            return new KeyValuePair<string, string>(login,role.Role.StringValue);
        }

        public async Task<Dictionary<string,string>> GetMemberRoles(string org)
        {
            var client = _clientFactory.Create();
            var installation = await client.GitHubApps.GetOrganizationInstallationForCurrent(org);

            var orgClient = await _clientFactory.Create(installation.Id);
            var members = await orgClient.Organization.Member.GetAll(org);

            var roleTasks = members
                .Select(m => orgClient.Organization.Member.GetOrganizationMembership(org, m.Login));

            var roles = await Task.WhenAll(roleTasks);
            var rolesByUser = roles
                .ToDictionary(r => r.User.Login, r => r.Role.StringValue);

            return rolesByUser;
        }

        public async Task<Dictionary<long,string>> GetRepositories(string org)
        {
            var client = _clientFactory.Create();
            var installation = await client.GitHubApps.GetOrganizationInstallationForCurrent(org);

            var orgClient = await _clientFactory.Create(installation.Id);
            var repositories = await orgClient.Repository.GetAllForOrg(org);


            var result = repositories
                .ToDictionary(r => r.Id, r => r.Name);

            return result;
        }

        public async Task<List<string>> GetRepositoryCollaborators(string org, long repositoryId)
        {
            var client = _clientFactory.Create();
            var installation = await client.GitHubApps.GetOrganizationInstallationForCurrent(org);

            var orgClient = await _clientFactory.Create(installation.Id);
            var collaborators = await orgClient.Repository.Collaborator.GetAll(repositoryId);

            var result = collaborators
                .Select(m => m.Login)
                .ToList();

            return result;
        }

        public async Task<Dictionary<string,string>> GetRepositoryCollaboratorPermissions(string org, long repositoryId)
        {
            var client = _clientFactory.Create();
            var installation = await client.GitHubApps.GetOrganizationInstallationForCurrent(org);

            var orgClient = await _clientFactory.Create(installation.Id);

            var collaborators = await orgClient.Repository.Collaborator.GetAll(repositoryId);

            var permissionsTasks = collaborators
                .Select(c => orgClient.Repository.Collaborator.ReviewPermission(repositoryId, c.Login));

            var permissions = await Task.WhenAll(permissionsTasks);

            var result = permissions
                .ToDictionary(p => p.User.Login, p => p.Permission.StringValue);

            return result;
        }
    }
}