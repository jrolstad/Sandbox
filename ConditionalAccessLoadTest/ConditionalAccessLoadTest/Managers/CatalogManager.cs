using ConditionalAccessLoadTest.Factories;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConditionalAccessLoadTest.Managers
{
    public class CatalogManager
    {
        private readonly Factories.GraphClientFactory _graphClientFactory;

        public CatalogManager(Factories.GraphClientFactory graphClientFactory)
        {
            _graphClientFactory = graphClientFactory;
        }
        public async Task<List<AccessPackageCatalog>> Get()
        {
            var client = _graphClientFactory.Create();

            var data = await client.IdentityGovernance.EntitlementManagement
                .AccessPackageCatalogs
                .Request()
                .GetAsync();

            return data.CurrentPage?.ToList();
        }

        public async Task<AccessPackageCatalog> Get(string id)
        {
            var client = _graphClientFactory.Create();

            var data = await client.IdentityGovernance.EntitlementManagement
                .AccessPackageCatalogs[id]
                .Request()
                .GetAsync();

            return data;
        }
    }
}
