using MedLink.Application.DTOs.Identity;
using MedLink.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Team_B.Controllers
{

    public class AuthController : BaseApiController
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthModel>> RegisterAsync([FromBody] RegisterModel registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(registerDto);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<ActionResult<AuthModel>> GetTokenAsync([FromBody] RequestTokenModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

    }
}
