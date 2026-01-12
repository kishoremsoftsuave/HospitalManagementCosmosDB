using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HospitalManagementCosmosDB.Domain.Entities
{
    public class Patient
    {
        //public string Id { get; set; } = Guid.NewGuid().ToString("N");// remove hyphens for Cosmos DB id
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Disease { get; set; } = string.Empty;
    }
}
