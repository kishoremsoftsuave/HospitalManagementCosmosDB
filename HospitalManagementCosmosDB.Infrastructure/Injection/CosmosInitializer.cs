using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HospitalManagementCosmosDB.Infrastructure.Injection
{
    public class CosmosInitializer
    {
        public static async Task InitializeAsync(CosmosClient client, CosmosDbOptions options)
        {
            var db = await client.CreateDatabaseIfNotExistsAsync(options.DatabaseId);
            foreach (var item in options.Containers)
            {
                await db.Database.CreateContainerIfNotExistsAsync(id: item.ContainerId, partitionKeyPath: item.PartitionKeyPath, throughput: 400);
                Console.WriteLine($"Ensured container: {item.ContainerId}");
            }
        }
    }
}
