using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagementCosmosDB.Infrastructure.Injection
{
    public class CosmosDbOptions
    {
        public string AccountEndpoint { get; set; } = null!;
        public string AccountKey { get; set; } = null!;
        public string DatabaseId { get; set; } = null!;
        public string ContainerId { get; set; } = null!;
        public string PartitionKeyPath { get; set; } = "/id";
    }
}
