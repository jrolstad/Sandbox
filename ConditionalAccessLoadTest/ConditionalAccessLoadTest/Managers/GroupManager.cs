﻿using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConditionalAccessLoadTest.Managers
{
    public class GroupManager
    {
        private readonly Factories.GraphClientFactory _graphClientFactory;

        public GroupManager(Factories.GraphClientFactory graphClientFactory)
        {
            _graphClientFactory = graphClientFactory;
        }

        public Task<Group> Get(string id)
        {
            var client = _graphClientFactory.Create();

            return client.Groups[id]
                .Request()
                .GetAsync();
        }

        public async Task<List<AppRoleAssignment>> GetAppRoles(string id)
        {
            var client = _graphClientFactory.Create();

            var request = client.Groups[id]
                .AppRoleAssignments
                .Request();

            var result = await request.GetAsync();

            var data = new List<AppRoleAssignment>();
            data.AddRange(result.CurrentPage);
            do
            {

                if (result.NextPageRequest != null)
                {
                    result = await result.NextPageRequest.GetAsync();
                }
                data.AddRange(result.CurrentPage);

            } while (result.NextPageRequest != null);


            return data;
        }

        
    }
}
