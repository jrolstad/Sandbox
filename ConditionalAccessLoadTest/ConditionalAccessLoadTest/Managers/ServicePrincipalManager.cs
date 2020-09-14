using Microsoft.Graph;
using System;
using System.Collections.Generic;
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
