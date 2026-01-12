using HospitalManagementCosmosDB.Application.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagementCosmosDB.Application.Interfaces
{
    public interface IPatientService
    {
        Task<List<PatientDTO>> GetAll();
        Task<PatientDTO?> GetById(string id);
        Task<PatientDTO> Create(CreatePatientDTO dto);
        Task<PatientDTO> UpdateById(UpdatePatientDTO dto);
        Task Delete(string id);
    }
}
