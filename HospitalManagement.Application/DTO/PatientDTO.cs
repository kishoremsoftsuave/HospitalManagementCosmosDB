using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagementCosmosDB.Application.DTO
{
    public class CreatePatientDTO
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Disease { get; set; } = string.Empty;
    }

    // DTO used for returning patient data
    public class PatientDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Disease { get; set; } = string.Empty;
    }

    // DTO used for updating a patient
    public class UpdatePatientDTO
    {
        public string Id { get; set; } = string.Empty; // Must match existing patient
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Disease { get; set; } = string.Empty;
    }
}
