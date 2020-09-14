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
                

            var result = await request.GetAsync();

            var appData = new List<Application>();
            appData.AddRange(result.CurrentPage);
            do
            {
               
                if (result.NextPageRequest != null)
                {
                    result = await result.NextPageRequest.GetAsync();
                }
                appData.AddRange(result.CurrentPage);

                if (top.HasValue && appData.Count > top) break;
            } while (result.NextPageRequest != null);

            

            if (top.HasValue) return appData.Take(top.Value).ToList();

            return appData;

                   
        }
    }
}
