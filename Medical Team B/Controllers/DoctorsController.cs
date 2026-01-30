using MedLink.Application.DTOs.Doctors;
using MedLink.Application.Interfaces.Services;
using MedLink.Application.DTOs.Doctors;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Team_B.Controllers
{
    /// <summary>
    /// Manages doctor-related operations.
    /// </summary>
    public class DoctorsController : BaseApiController
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        /// <summary>
        /// Searches for doctors based on criteria.
        /// </summary>
        /// <param name="searchParams">Filters for doctor search (e.g., keyword, specialty, location, date).</param>
        [HttpGet("search")]
        public async Task<ActionResult<IReadOnlyList<DoctorSearchResultDto>>> Search(
            [FromQuery] DoctorSearchParams searchParams)
        {
            var doctors = await _doctorService.SearchDoctorsAsync(searchParams);
            return Ok(doctors);
        }

    }
}
