using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagementCosmosDB.Infrastructure.Injection
{
    public class CosmosInitializer
    {
        public static async Task InitializeAsync(CosmosClient client, string databaseId, string containerId, string partitionKeyPath)
        {
            var db = await client.CreateDatabaseIfNotExistsAsync(databaseId);
            await db.Database.CreateContainerIfNotExistsAsync(id: containerId, partitionKeyPath: partitionKeyPath, throughput: 400);
        }
    }
}
