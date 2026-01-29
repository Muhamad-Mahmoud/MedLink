using MedLink.Domain.Entities.User;
using MedLink_Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Team_B.Controllers
{
    public class UserProfilesController:BaseApiController
    {

            private readonly IUserProfileService _userProfileService;

            public UserProfilesController(IUserProfileService userProfileService)
            {
                _userProfileService = userProfileService;
            }

            // GET: api/userprofiles/{id}
            [HttpGet("{id:guid}")]
            public async Task<IActionResult> GetById(Guid id)
            {
                var profile = await _userProfileService.GetByIdAsync(id);

                if (profile == null)
                    return NotFound();

                return Ok(profile);
            }

            // GET: api/userprofiles?pageIndex=1&pageSize=10
            [HttpGet]
            public async Task<IActionResult> GetAll(
                [FromQuery] int pageIndex = 1,
                [FromQuery] int pageSize = 10)
            {
                var profiles = await _userProfileService.GetAllAsync(pageIndex, pageSize);
                return Ok(profiles);
            }

            // POST: api/userprofiles
            [HttpPost]
            public async Task<IActionResult> Create([FromBody] UserProfile profile)
            {
                var createdProfile = await _userProfileService.AddAsync(profile);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdProfile.Id },
                    createdProfile
                );
            }

            // PUT: api/userprofiles/{id}
            [HttpPut("{id:guid}")]
            public async Task<IActionResult> Update(int id, [FromBody] UserProfile profile)
            {
            if (id != profile.Id)
                return BadRequest("Id mismatch");

            var updatedProfile = await _userProfileService.UpdateAsync(profile);
                return Ok(updatedProfile);
            }
        }
    }



