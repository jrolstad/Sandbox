using ConditionalAccessLoadTest.Factories;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConditionalAccessLoadTest.Managers
{
    public class ApplicationManager
    {
        private readonly Factories.GraphClientFactory _graphClientFactory;

        public ApplicationManager(Factories.GraphClientFactory graphClientFactory)
        {
            _graphClientFactory = graphClientFactory;
        }

        public async Task<string> Create(string name)
        {
            var client = _graphClientFactory.Create();

            var application = new Application { DisplayName = name };

            var newApp = await client.Applications.
                Request()
                .AddAsync(application);

            return newApp.Id;
        }
        public async Task Delete(string objectId)
        {
            var client = _graphClientFactory.Create();

            await client.Applications[objectId]
                .Request()
                .DeleteAsync();
        }


        public async Task<List<Application>> Get(int? top=null)
        {
            var client = _graphClientFactory.Create();

            var request = client.Applications
                .Request()
                .Select("appId,id,displayName")
                .Top(999);
                
            var data = new List<Application>();
            do
            {
                var result = await request.GetAsync();
                data.AddRange(result.CurrentPage);

                request = result.NextPageRequest;
            } while (request != null);

            if (top.HasValue) return data.Take(top.Value).ToList();
            return data;

                   
        }
    }
}
