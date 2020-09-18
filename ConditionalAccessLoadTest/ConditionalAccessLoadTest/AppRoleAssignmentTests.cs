using ConditionalAccessLoadTest.Managers;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ConditionalAccessLoadTest
{
    public class AppRoleAssignmentTests
    {
        private string _tenantId = Environment.GetEnvironmentVariable("aad-dev-tenant-id", EnvironmentVariableTarget.User);
        private string _clientId = Environment.GetEnvironmentVariable("aad-dev-client-id", EnvironmentVariableTarget.User);
        private string _clientSecret = Environment.GetEnvironmentVariable("aad-dev-client-secret", EnvironmentVariableTarget.User);
        private readonly ITestOutputHelper _testOutputHelper;

        public AppRoleAssignmentTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData("b115cc80-5deb-46df-bf46-5147d120fcd5")]
        public async Task AssignGroup_LoadTestServicePrincipals_DoesNotFail(string groupId)
        {
            var factory = new Factories.GraphClientFactory(_tenantId, _clientId, _clientSecret);
            var appManager = new ApplicationManager(factory);
            var servicePrincipalManager = new ServicePrincipalManager(factory);
            var groupManager = new GroupManager(factory);

            var servicePrincipals = await GetLoadTestServicePrincipals(appManager, servicePrincipalManager);
            var servicePrincipalIds = servicePrincipals
                .Select(s => s.Id)
                .Distinct()
                .ToList();

            var addGroupTasks = servicePrincipalIds
                .Select(s => servicePrincipalManager.AssignGroupToDefaultRole(s, new List<string> { groupId }));

            await Task.WhenAll(addGroupTasks);

            var assignments = await groupManager.GetAppRoles(groupId);
            Assert.Equal(servicePrincipalIds.Count, assignments.Select(a=>a.Id).Distinct().Count());

        }

        [Theory]
        [InlineData("b115cc80-5deb-46df-bf46-5147d120fcd5")]
        public async Task GetGroupAssignments(string groupId)
        {
            var factory = new Factories.GraphClientFactory(_tenantId, _clientId, _clientSecret);
            var groupManager = new GroupManager(factory);

            var assignments = await groupManager.GetAppRoles(groupId);
            _testOutputHelper.WriteLine($"Group Assignment Count:{assignments.Count}");
        }

        [Fact]
        public async Task GetServicePrincipals()
        {
            var factory = new Factories.GraphClientFactory(_tenantId, _clientId, _clientSecret);
            var servicePrincipalManager = new ServicePrincipalManager(factory);

            var servicePrincipals = await servicePrincipalManager.Get();
            _testOutputHelper.WriteLine($"Service Principal Count:{servicePrincipals.Count}");
        }

        private static async Task<List<ServicePrincipal>> GetLoadTestServicePrincipals(ApplicationManager appManager, ServicePrincipalManager servicePrincipalManager)
        {
            var applications = await GetLoadTestApplications(appManager);

            var servicePrincipals = await servicePrincipalManager.Get();
            var applicationsByAppId = applications.ToLookup(a => a.AppId);

            return servicePrincipals
                .Where(sp => applicationsByAppId.Contains(sp.AppId))
                .ToList();
        }
        private static async Task<List<Application>> GetLoadTestApplications(ApplicationManager appManager)
        {
            var applications = await appManager.Get();
            var loadTestApps = applications
                .Where(a => !string.IsNullOrWhiteSpace(a.DisplayName))
                .Where(a => a.DisplayName.Contains("jorolsta-load-test"))
                .ToList();
            return loadTestApps;
        }
    }

   
}
