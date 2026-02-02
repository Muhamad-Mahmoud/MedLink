using MedLink.Application.Interfaces.Services;
using MedLink.Domain.Entities.Content;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Team_B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly IAboutService _aboutService;

        public AboutController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

       
        [HttpGet("{id:int}", Name = "GetAboutById")]
        public async Task<ActionResult<About>> GetById(int id)
        {
            var about = await _aboutService.GetAboutByIdAsync(id);
            if (about == null) return NotFound();

            return Ok(about);
        }

       
        [HttpPost]
        public async Task<ActionResult<About>> Create(About about)
        {
           
            about.LastUpdated = DateTime.UtcNow;

            var createdAbout = await _aboutService.AddAboutAsync(about);

            return CreatedAtRoute("GetAboutById", new { id = createdAbout.Id }, createdAbout);
        }

       
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, About about)
        {
            if (id != about.Id) return BadRequest("ID mismatch");

            about.LastUpdated = DateTime.UtcNow; 
            await _aboutService.UpdateAboutAsync(about);
            return NoContent();
        }

       
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _aboutService.DeleteAboutAsync(id);
            return NoContent();
        }
    }
}
