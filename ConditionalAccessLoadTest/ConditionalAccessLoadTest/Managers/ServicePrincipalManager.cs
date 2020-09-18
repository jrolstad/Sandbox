using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConditionalAccessLoadTest.Managers
{
    public class ServicePrincipalManager
    {
        private readonly Factories.GraphClientFactory _graphClientFactory;

        public ServicePrincipalManager(Factories.GraphClientFactory graphClientFactory)
        {
            _graphClientFactory = graphClientFactory;
        }

        public async Task<ServicePrincipal> Create(Application application)
        {
            var servicePrincipal = new ServicePrincipal
            {
                AppId = application.AppId,
                DisplayName = application.DisplayName,
            };

            var client = _graphClientFactory.Create();
            var result = await client.ServicePrincipals
                .Request()
                .AddAsync(servicePrincipal);

            return result;
        }

        public Task<ServicePrincipal> Update(ServicePrincipal toUpdate)
        {
            var client = _graphClientFactory.Create();
            return client.ServicePrincipals[toUpdate.Id]
                .Request()
                .UpdateAsync(toUpdate);
        }

        public async Task AssignGroupToDefaultRole(string servicePrincipalId,List<string> groupPrincipalIds)
        {
            var client = _graphClientFactory.Create();
            var existing = await client.ServicePrincipals[servicePrincipalId]
                       .AppRoleAssignedTo
                       .Request()
                       .GetAsync();
            if (existing.Any(a => groupPrincipalIds.Contains(a.PrincipalId.ToString()))) return;

            var appRoleAssignmentTasks = groupPrincipalIds
                .Select(id =>
                {

                    const string groupPrincipalType = "Group";
                    var defaultAppRoleId = Guid.Parse("00000000-0000-0000-0000-000000000000");

                    var assignment = new AppRoleAssignment
                    {
                        PrincipalId = Guid.Parse(id),
                        PrincipalType = groupPrincipalType, 
                        ResourceId = Guid.Parse(servicePrincipalId),
                        AppRoleId = defaultAppRoleId

                    };
                    return client.ServicePrincipals[servicePrincipalId]
                        .AppRoleAssignedTo
                        .Request()
                        .AddAsync(assignment);
                });

            await Task.WhenAll(appRoleAssignmentTasks);
        }

        public async Task<List<ServicePrincipal>> Get()
        {
            var client = _graphClientFactory.Create();

            var request = client.ServicePrincipals
                .Request()
                .Select("appId,id,displayName")
                .Top(999);


            var result = await request.GetAsync();

            var appData = new List<ServicePrincipal>();
            appData.AddRange(result.CurrentPage);
            do
            {

                if (result.NextPageRequest != null)
                {
                    result = await result.NextPageRequest.GetAsync();
                }
                appData.AddRange(result.CurrentPage);

            } while (result.NextPageRequest != null);


            return appData;

        }
    }
}
