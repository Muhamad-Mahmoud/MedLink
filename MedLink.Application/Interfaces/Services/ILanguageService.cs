using MedLink.Application.Interfaces.Specifications;
using MedLink.Domain.Entities.Content;
using MedLink.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Interfaces.Services
{
    public interface ILanguageService
    {
        Task<Language?> GetLanguageByIdAsync(int id);
        Task<IReadOnlyList<Language>> GetAllLanguagesAsync(ISpecification<Language>? spec = null);
        Task<Language> AddLanguageAsync(Language language);
        Task UpdateLanguageAsync(Language language);
        Task DeleteLanguageAsync(int id);
    }
}
