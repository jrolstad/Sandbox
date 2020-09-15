using ConditionalAccessLoadTest.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ConditionalAccessLoadTest
{
    public class ELMTests
    {
        private string _tenantId = Environment.GetEnvironmentVariable("aad-dev-tenant-id", EnvironmentVariableTarget.User);
        private string _clientId = Environment.GetEnvironmentVariable("aad-dev-client-id", EnvironmentVariableTarget.User);
        private string _clientSecret = Environment.GetEnvironmentVariable("aad-dev-client-secret", EnvironmentVariableTarget.User);

        private readonly ITestOutputHelper _testOutputHelper;

        public ELMTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task GetCatalogs()
        {
            var factory = new Factories.GraphClientFactory(_tenantId, _clientId, _clientSecret);
            var catalogManager = new CatalogManager(factory);

            var catalogs = await catalogManager.Get();

            Assert.NotEmpty(catalogs);

        }

        [Theory]
        [InlineData("8f67737c-48bc-4bc4-b029-c63d17d5664a")]
        public async Task GetCatalog(string catalogId)
        {
            var factory = new Factories.GraphClientFactory(_tenantId, _clientId, _clientSecret);
            var catalogManager = new CatalogManager(factory);

            var catalog = await catalogManager.Get(catalogId);

            Assert.NotNull(catalog);

        }
    }
}
