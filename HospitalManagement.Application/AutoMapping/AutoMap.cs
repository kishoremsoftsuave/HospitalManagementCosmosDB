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
            CreateMap<CreatePatientDTO, Patient>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()));
            CreateMap<UpdatePatientDTO, Patient>();
            CreateMap<Patient, PatientDTO>();
        }
  }
}
