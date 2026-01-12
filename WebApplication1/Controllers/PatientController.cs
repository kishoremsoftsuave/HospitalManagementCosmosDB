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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var patient = await _service.GetById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePatientDTO dto)
        {
            var createdPatient = await _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdPatient.Id }, createdPatient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdatePatientDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest("ID mismatch");
            }
            var updatedPatient = await _service.UpdateById(dto);
            return Ok(updatedPatient);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
