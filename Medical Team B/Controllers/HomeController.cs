using MedLink.Application.DTOs.Doctors;
using MedLink.Application.DTOs.Medical;
using MedLink.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Team_B.Controllers
{
    /// <summary>
    /// Handles home page related data.
    /// </summary>
    public class HomeController : BaseApiController
    {
        private readonly ISpecializationService _specializationService;
        private readonly IDoctorService _doctorService;

        public HomeController(ISpecializationService specializationService, IDoctorService doctorService)
        {
            _specializationService = specializationService;
            _doctorService = doctorService;
        }

        /// <summary>
        /// Retrieves medical specializations.
        /// </summary>
        /// <param name="count">Optional number of specializations to return.</param>
        [HttpGet("specialties")]
        public async Task<ActionResult<IReadOnlyList<SpecializationDto>>> GetSpecialties([FromQuery] int? count)
        {
            var specialties = await _specializationService.GetAllSpecializationsAsync(count);
            return Ok(specialties);
        }

        /// <summary>
        /// Retrieves top-rated doctors.
        /// </summary>
        /// <param name="count">Number of top doctors to return (default is 3).</param>
        [HttpGet("top-doctors")]
        public async Task<ActionResult<IReadOnlyList<DoctorSearchResultDto>>> GetTopDoctors([FromQuery] int count = 3)
        {
            var topDoctors = await _doctorService.GetTopRatedDoctorsAsync(count);
            return Ok(topDoctors);
        }
    }
}
