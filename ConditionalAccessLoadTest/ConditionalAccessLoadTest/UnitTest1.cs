using ConditionalAccessLoadTest.Managers;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ConditionalAccessLoadTest
{
    public class UnitTest1
    {
        private string _tenantId = "81c98a12-bb40-4777-8e14-908eafa46fa7";
        private string _clientId = "9e1a841e-057c-4a2e-9ecf-e3688b298766";
        private string _clientSecret = "<secret here>";

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
            Assert.Equal(1, applications.Count);
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
        [InlineData("4a651cd4-46a4-4350-8cc4-e2395ee15e8a", 254)]
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
    }
}
