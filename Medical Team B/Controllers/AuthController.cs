using MedLink.Application.DTOs.Identity;
using MedLink.Application.DTOs.Identity;
using MedLink.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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





        [Authorize(Roles = "Admin")]
        [HttpPost("addrole")]
        public async Task<ActionResult<string>> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(model);

            return !string.IsNullOrEmpty(result) ? BadRequest(result) : Ok(model);
        }





        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var isVerificationEmailSent = await _authService.ForgotPasswordAsync(model);

            if (!isVerificationEmailSent)
                return BadRequest("Invalid Request");
            return Ok();



        }




        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _authService.ResetPasswordAsync(model);

            if (!result.Success)
                return BadRequest(new { Errors = result.Message });
            return Ok(result.Message);
        }

        [HttpPost("verify-phone/send")]
        public async Task<IActionResult> SendPhoneVerification([FromBody] SendPhoneVerificationModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.SendPhoneVerificationAsync(model.Email, model.PhoneNumber);
            return Ok(result);
        }

        [HttpPost("verify-phone/confirm")]
        public async Task<IActionResult> ConfirmPhone([FromBody] ConfirmPhoneModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.ConfirmPhoneNumberAsync(model.Email, model.Code, model.PhoneNumber);
            return Ok(result);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
                return BadRequest("User ID and Code are required");

            var result = await _authService.ConfirmEmailAsync(userId, code);
            return Ok(result);
        }

        [HttpGet("signin-google")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Auth");
            var properties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, Microsoft.AspNetCore.Authentication.Google.GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<ActionResult<AuthModel>> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(Microsoft.AspNetCore.Authentication.Google.GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return BadRequest("Google authentication failed");

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Name)?.Value;
            var googleId = claims?.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (email == null || googleId == null)
                return BadRequest("Could not retrieve email or Google ID");

            var authModel = await _authService.LoginWithGoogleAsync(email, name, googleId);
            return Ok(authModel);
        }

        [Authorize]
        [HttpDelete("account")]
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (userId == null) return Unauthorized();

            var result = await _authService.DeleteAccountAsync(userId);
            return Ok(result);
        }

        [HttpPost("restore-account")]
        public async Task<ActionResult<AuthModel>> RestoreAccountAsync([FromBody] RequestTokenModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RestoreAccountAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}
