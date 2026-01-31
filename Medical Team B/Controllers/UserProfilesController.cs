using MedLink_Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MedLink.API.Controllers
{
    [ApiController]
    [Route("api/user/profile")]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public UserProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetMyDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var dashboard = await _profileService.GetMyDashboardAsync(userId);

            return Ok(dashboard);
        }
    }
}



