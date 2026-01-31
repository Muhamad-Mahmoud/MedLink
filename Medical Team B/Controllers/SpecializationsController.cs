using MedLink.Application.Interfaces.Services;
using MedLink.Application.Specifications;
using MedLink.Domain.Entities.Medical;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Team_B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationsController : ControllerBase
    {
        private readonly ISpecializationService _specializationService;

        public SpecializationsController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }

      
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Specialization>>> GetAll()
        {
            var spec = new BaseSpecification<Specialization>(s => true);
            var result = await _specializationService.GetAllSpecializationsAsync(spec);
            return Ok(result);
        }

        [HttpGet("with-doctors")]
        public async Task<ActionResult<IReadOnlyList<Specialization>>> GetAllWithDoctors()
        {
            // 1. إنشاء الـ Spec
            var spec = new BaseSpecification<Specialization>(s => true);

            // 2. إضافة الـ Include بالطريقة اللي الـ BaseSpecification بتفهمها
            // سطر واحد بسيط من غير كلمة .Include()
            spec.Includes.Add(s => s.Doctors);

            var result = await _specializationService.GetAllSpecializationsAsync(spec);
            return Ok(result);
        }


        [HttpGet("{id:int}", Name = "GetSpecializationById")]
        public async Task<ActionResult<Specialization>> GetById(int id) // تغيير Guid لـ int
        {
            var specialization = await _specializationService.GetSpecializationByIdAsync(id);
            if (specialization == null)
                return NotFound();

            return Ok(specialization);
        }

       
        [HttpPost]
        public async Task<ActionResult<Specialization>> Create([FromBody] Specialization specialization)
        {
            var created = await _specializationService.CreateSpecializationAsync(specialization);
           
            return CreatedAtAction(nameof(GetById), new { id = specialization.Id }, specialization);
        }


       
        [HttpPut("{id:int}")] 
        public async Task<IActionResult> Update(int id, [FromBody] Specialization specialization)
        {
            if (id != specialization.Id) // Both are now ints
                return BadRequest("ID mismatch");

            await _specializationService.UpdateSpecializationAsync(specialization);
            return NoContent();
        }

        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) 
        {
            await _specializationService.DeleteSpecializationAsync(id);
            return NoContent();
        }
    }
}
