using MedLink_Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Medical_Team_B.Controllers
{
    [ApiController]
    [Route("api/languages")]
    public class LanguagesController : ControllerBase
    {
        private readonly ILanguageService _languageService;

        public LanguagesController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_languageService.GetAllLanguages());
        }
    }

}
