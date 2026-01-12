using HospitalManagementCosmosDB.Application.DTO;
using HospitalManagementCosmosDB.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementCosmosDB.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _service;
        public PatientController(IPatientService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var patients = await _service.GetAll();
            return Ok(patients);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var patient = await _service.GetById(id);
            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePatientDTO dto)
        {
            var createdPatient = await _service.Create(dto);
            return CreatedAtAction(nameof(GetById), createdPatient);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateById(string id, [FromBody] UpdatePatientDTO dto)
        {
            var updatedPatient = await _service.UpdateById(id, dto);
            return Ok("Patient Details is Updated");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(string id)
        {
            await _service.Delete(id);
            return Ok("Patient Details is Deleted");
        }
    }
}
