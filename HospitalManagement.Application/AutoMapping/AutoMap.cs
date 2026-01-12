using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using HospitalManagementCosmosDB.Application.DTO;
using HospitalManagementCosmosDB.Domain.Entities;
namespace HospitalManagementCosmosDB.Application.AutoMapping
{
  public class AutoMap : Profile
  {
        public AutoMap()
        {
            CreateMap<CreatePatientDTO, Patient>();
            CreateMap<UpdatePatientDTO, Patient>();
            CreateMap<Patient, PatientDTO>();
        }
  }
}
