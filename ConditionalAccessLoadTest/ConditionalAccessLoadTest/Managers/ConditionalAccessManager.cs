using ConditionalAccessLoadTest.Factories;
using Microsoft.Graph;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConditionalAccessLoadTest.Managers
{
    public class ConditionalAccessManager
    {
        private readonly Factories.GraphClientFactory _graphClientFactory;

        public ConditionalAccessManager(Factories.GraphClientFactory graphClientFactory)
        {
            _graphClientFactory = graphClientFactory;
        }
        public async Task AddApplicationToConditionalAccessPolicy(string policyId, List<string> applications)
        {
            var client = _graphClientFactory.Create();

            var accessPolicy = await client.Policies.ConditionalAccessPolicies[policyId]
                .Request()
                .GetAsync();

            var existingApplications = accessPolicy
                .Conditions?
                .Applications?
                .IncludeApplications ?? new List<string>();

            var appList = existingApplications
                .Union(applications)
                .Distinct()
                .ToList();

            var updatedPolicy = new ConditionalAccessPolicy
            {
                Conditions = new ConditionalAccessConditionSet
                {
                    Applications = new ConditionalAccessApplications
                    {
                        IncludeApplications = applications,
                        ExcludeApplications = new List<string>(),
                        IncludeUserActions = new List<string>(),
                        ODataType = null
                    },
                    ODataType = null
                },
                ODataType = null
            };

            var result = JsonConvert.SerializeObject(updatedPolicy);

            await client.Policies.ConditionalAccessPolicies[policyId]
                .Request()
                .UpdateAsync(updatedPolicy);
        }
    }
}
