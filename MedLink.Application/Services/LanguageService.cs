using MedLink.Application.Interfaces.Persistence;
using MedLink.Application.Interfaces.Services;
using MedLink.Application.Interfaces.Specifications;
using MedLink.Domain.Entities.Content;
using MedLink.Domain.Entities.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedLink.Application.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly IUnitOfWork _unitOfWork;
        public LanguageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Language> AddLanguageAsync(Language language)
        {
            await _unitOfWork.Repository<Language>().AddAsync(language);
            await _unitOfWork.Complete();
            return language;
        }

        public async Task DeleteLanguageAsync(int id)
        {
            var repo = _unitOfWork.Repository<Language>();
            var entity = await repo.GetByIdAsync(id);
            if (entity != null)
            {
                repo.Delete(entity);
                await _unitOfWork.Complete();
            }
        }

        public async Task<IReadOnlyList<Language>> GetAllLanguagesAsync(ISpecification<Language>? spec = null)
        {
            var repo = _unitOfWork.Repository<Language>();
            return spec != null
                ? await repo.GetAllWithSpecAsync(spec)
                : await repo.GetAllAsync();
        }

        public async Task<Language?> GetLanguageByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<Language>();
            return await repo.GetByIdAsync(id);
        }

        public async Task UpdateLanguageAsync(Language language)
        {
            var repo = _unitOfWork.Repository<Language>();
            repo.Update(language);
            await _unitOfWork.Complete();
        }
    }
}
