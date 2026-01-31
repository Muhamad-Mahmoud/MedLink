using MedLink.Application.DTOs.Medical;

namespace MedLink.Application.Interfaces.Services
{
    public interface ISpecializationService
    {
        Task<IReadOnlyList<SpecializationDto>> GetAllSpecializationsAsync(int? count = null);
    }
}
