using MedLink.Application.DTOs.Doctors;
using MedLink.Application.Interfaces.Services;
using MedLink_Application.DTOs.Doctors;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Team_B.Controllers
{
    public class DoctorsController : BaseApiController
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IReadOnlyList<DoctorSearchResultDto>>> Search(
            [FromQuery] DoctorSearchParams searchParams)
        {
            var doctors = await _doctorService.SearchDoctorsAsync(searchParams);
            return Ok(doctors);
        }
    }
}
