using AutoMapper;
using MedLink.Application.DTOs.Medical;
using MedLink.Application.Interfaces.Services;
using MedLink.Application.Specifications.Medical;
using MedLink.Domain.Entities.Medical;
using MedLink_Application.Interfaces.Persistence;

namespace MedLink.Application.Services
{
    public class SpecializationService : ISpecializationService
    {
        private readonly IGenericRepository<Specialization> _specializationRepo;
        private readonly IMapper _mapper;

        public SpecializationService(IGenericRepository<Specialization> specializationRepo, IMapper mapper)
        {
            _specializationRepo = specializationRepo;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<SpecializationDto>> GetAllSpecializationsAsync(int? count = null)
        {
            var spec = new GetAllSpecialtiesSpec(count);
            var specializations = await _specializationRepo.GetAllWithSpecAsync(spec);

            return _mapper.Map<IReadOnlyList<SpecializationDto>>(specializations);
        }
    }
}
