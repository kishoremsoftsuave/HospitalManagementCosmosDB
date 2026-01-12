using AutoMapper;
using HospitalManagementCosmosDB.Application.DTO;
using HospitalManagementCosmosDB.Application.Interfaces;
using HospitalManagementCosmosDB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalManagementCosmosDB.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _repo;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<PatientDTO>> GetAll()
        {
            var patients = await _repo.GetAll();
            return _mapper.Map<List<PatientDTO>>(patients);
        }

        public async Task<PatientDTO?> GetById(string id)
        {
            var patient = await _repo.GetById(id);
            if (patient == null) return null;

            return _mapper.Map<PatientDTO>(patient);
        }
        public async Task<PatientDTO> Create(CreatePatientDTO dto)
        {
            var patient = _mapper.Map<Patient>(dto);
            var created = await _repo.Create(patient);

            return _mapper.Map<PatientDTO>(created);
        }

        public async Task<PatientDTO> UpdateById(UpdatePatientDTO dto)
        {
            var patient = _mapper.Map<Patient>(dto);

            var updated = await _repo.UpdateById(patient);

            return _mapper.Map<PatientDTO>(updated);
        }

        public async Task Delete(string id)
        {
            await _repo.Delete(id);
        }
    }
}
