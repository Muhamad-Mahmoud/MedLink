using AutoMapper;
using MedLink.Application.DTOs.Doctors;
using MedLink.Application.Interfaces.Services;
using MedLink.Application.Specifications.Medical;
using MedLink.Application.Specifications.Medical;
using MedLink.Domain.Entities.Medical;
using MedLink.Application.DTOs.Doctors;
using MedLink.Application.Interfaces.Persistence;

namespace MedLink.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IGenericRepository<Doctor> _doctorRepo;
        private readonly IMapper _mapper;

        public DoctorService(IGenericRepository<Doctor> doctorRepo, IMapper mapper)
        {
            _doctorRepo = doctorRepo;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<DoctorSearchResultDto>> SearchDoctorsAsync(DoctorSearchParams searchParams)
        {
            var spec = new DoctorSearchSpec(searchParams);
            var doctors = await _doctorRepo.GetAllWithSpecAsync(spec);

            return _mapper.Map<IReadOnlyList<DoctorSearchResultDto>>(doctors);
        }

        public async Task<IReadOnlyList<DoctorSearchResultDto>> GetTopRatedDoctorsAsync(int count)
        {
            var spec = new TopRatedDoctorsSpec(count);
            var doctors = await _doctorRepo.GetAllWithSpecAsync(spec);
            return _mapper.Map<IReadOnlyList<DoctorSearchResultDto>>(doctors);
        }
    }
}
