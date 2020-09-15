using ConditionalAccessLoadTest.Managers;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ConditionalAccessLoadTest
{
    public class ConditionalAccessTests
    {
        private string _tenantId = Environment.GetEnvironmentVariable("aad-dev-tenant-id",EnvironmentVariableTarget.User);
        private string _clientId = Environment.GetEnvironmentVariable("aad-dev-client-id", EnvironmentVariableTarget.User);
        private string _clientSecret = Environment.GetEnvironmentVariable("aad-dev-client-secret", EnvironmentVariableTarget.User);

        private readonly ITestOutputHelper _testOutputHelper;

        public ConditionalAccessTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        [Theory]
        //[InlineData("1",1000)]
        //[InlineData("2",1000)]
        //[InlineData("3",1000)]
        //[InlineData("4",500)]
        [InlineData("5",500)]
        public async Task CreatApplications(string set,int appCount)
        {
            var factory = new Factories.GraphClientFactory(_tenantId, _clientId, _clientSecret);
            var appManager = new ApplicationManager(factory);

            var createTasks = new List<Task<string>>();
            for(var i = 0;i<appCount;i++)
            {
                var appName = $"jorolsta-load-test-{set}-{i}";
                createTasks.Add(appManager.Create(appName));
            }

            Task.WaitAll(createTasks.ToArray());
        }

        [Fact]
        public async Task CreateServicePrincipals()
        {
            var factory = new Factories.GraphClientFactory(_tenantId, _clientId, _clientSecret);
            var appManager = new ApplicationManager(factory);
            var servicePrincipalManager = new ServicePrincipalManager(factory);

            var applications = await GetLoadTestApplications(appManager);

            var servicePrincipals = await servicePrincipalManager.Get();
            var servicePrincipalsByAppId = servicePrincipals
                .ToLookup(s => s.AppId);

            var appsMissingServicePrincipal = applications
                .Where(a => !servicePrincipalsByAppId.Contains(a.AppId))
                .ToList();

            var createTasks = appsMissingServicePrincipal
                .Select(a => servicePrincipalManager.Create(a))
                .ToArray();

            Task.WaitAll(createTasks);
        }

        [Fact]
        public async Task DeleteApplications()
        {
            var factory = new Factories.GraphClientFactory(_tenantId, _clientId, _clientSecret);
            var appManager = new ApplicationManager(factory);
            var applicationsToDelete = await GetLoadTestApplications(appManager);

            var deleteTasks = applicationsToDelete
                .Select(a => appManager.Delete(a.Id))
                .ToArray();

            Task.WaitAll(deleteTasks);
        }

        [Fact]
        public async Task Get_LoadTestApplicationCount()
        {
            var factory = new Factories.GraphClientFactory(_tenantId, _clientId, _clientSecret);
            var appManager = new ApplicationManager(factory);
            var applications = await GetLoadTestApplications(appManager);

            Assert.Equal(applications.Select(a=>a.Id).Count(), applications.Count);
            _testOutputHelper.WriteLine($"Number of applications: {applications.Count}");
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

        [Fact]
        public async Task Get_ByTop()
        {
            var factory = new Factories.GraphClientFactory(_tenantId, _clientId, _clientSecret);
            var appManager = new ApplicationManager(factory);

            var result = await appManager.Get(1);

            Assert.Single(result);
        }

        [Theory]
        [InlineData("4a651cd4-46a4-4350-8cc4-e2395ee15e8a", 255)]
        public async Task AssignAppsToPolicy(string policyId, int top)
        {
            var factory = new Factories.GraphClientFactory(_tenantId, _clientId, _clientSecret);
            var appManager = new ApplicationManager(factory);
            var policyManager = new ConditionalAccessManager(factory);

            var applications = await GetLoadTestApplications(appManager);
                
                
            var appIds = applications
                .Take(top)
                .Select(a => a.AppId)
                .ToList();
            await policyManager.AddApplicationToConditionalAccessPolicy(policyId, appIds);
        }

        [Fact]
        public void GetMaxPolicyDetailSize()
        {
            // Per code at https://dev.azure.com/msazure/One/_git/AD-madrid-MSGraph?path=%2Fsrc%2FIdentityProtectionServices.WebRole%2FProviders%2FPolicy%2FConditionalAccessPolicyManager.cs&_a=contents&version=GBmaster
            var maxSize = 1024 * 10 * sizeof(char);

            _testOutputHelper.WriteLine($"Maximum PolicyDetail message size: {maxSize}");
        }
    }
}
