using MedLink.Application.Interfaces.Services;
using MedLink.Infrastructure.Services;
using MedLink.Application.DTOs.UserProfile;
using MedLink.Application.Interfaces.Services;
using MedLink.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MedLink.API.Controllers
{
    [ApiController]
    [Route("api/profile")]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IProfileDashboardService _profileDashboardService;
        private readonly IUserLanguageService _userLanguageService;
        private readonly IProfileService _profileService;
        private readonly IImageService _imageService;
        private readonly IProfileAppointmentService _appointmentService;

        public UserProfileController(
            IProfileDashboardService profileDashboardService,
            IUserLanguageService userLanguageService,
            IProfileService profileService,
            IProfileAppointmentService appointmentService,
            IImageService imageService)
        {
            _profileDashboardService = profileDashboardService;
            _userLanguageService = userLanguageService;
            _profileService = profileService;
            _imageService = imageService;
            _appointmentService = appointmentService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue("uid") ?? throw new UnauthorizedAccessException("User not authenticated"); ; 
        }



        [HttpGet("dashboard")]
        public async Task<IActionResult> GetMyDashboard()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var dashboard = await _profileDashboardService.GetMyDashboardAsync(userId);
            return Ok(dashboard);
        }


        // api/profile/me
        [HttpGet("me")]
        public async Task<ActionResult<EditProfileDto>> GetMyProfile()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var profile = await _profileService.GetMyProfileAsync(
                userId);
            return Ok(profile);
        }

        //  api/profile
        [HttpPut]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _profileService.UpdateMyProfileAsync(userId, dto);
            return NoContent();
        }


        // PUT: api/profile/image
        [HttpPut("image")]
        public async Task<IActionResult> UpdateProfileImage([FromForm] IFormFile image)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (image == null || image.Length == 0)
                return BadRequest("Image is required");

            var imageUrl = await _imageService.UploadAsync(image);

            await _profileService.UpdateProfileImageAsync(userId, imageUrl);
            return NoContent();
        }


        // api/profile/image
        [HttpDelete("image")]
        public async Task<IActionResult> RemoveProfileImage()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _profileService.RemoveProfileImageAsync(userId);
            return NoContent();
        }


        //  api/profile/appointments/past

        [HttpGet("appointments/past")]
        public async Task<IActionResult> GetPast(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var userId = GetUserId();

            var result = await _appointmentService
                .GetPastAsync(userId, page, pageSize);

            return Ok(result);
        }

        //api/profile/appointments/past

        [HttpGet("appointments/upcoming")]
        public async Task<IActionResult> GetUpcoming(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var userId = GetUserId();

            var result = await _appointmentService
                .GetUpcomingAsync(userId, page, pageSize);

            return Ok(result);
        }


        // api/profile/language
        [HttpGet("language")]
        public async Task<IActionResult> GetPreferredLanguage()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var language = await _userLanguageService.GetPreferredLanguageAsync(userId);
            return Ok(language);
        }

        //  api/profile/language
        [HttpPut("language")]
        public async Task<IActionResult> UpdatePreferredLanguage([FromBody] UpdateLanguageDto dto)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _userLanguageService.UpdatePreferredLanguageAsync(userId, dto);
            return NoContent();
        }



    }
}

