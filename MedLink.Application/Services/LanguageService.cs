using MedLink_Application.DTOs.UserProfile;
using MedLink_Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink_Application.Services
{
    public class LanguageService : ILanguageService
    {
        private static readonly List<LanguageDto> Languages = new()
    {
        new() { Code = "en", Name = "English" },
        new() { Code = "ar", Name = "العربية" },
        new() { Code = "fr", Name = "Français" },
        new() { Code = "de", Name = "Deutsch" },
        new() { Code = "es", Name = "Español" },
        new() { Code = "ja", Name = "日本語" },
        new() { Code = "zh", Name = "中文" },
        new() { Code = "ru", Name = "Русский" }
    };

        public IReadOnlyList<LanguageDto> GetAllLanguages()
            => Languages;
    }

}
