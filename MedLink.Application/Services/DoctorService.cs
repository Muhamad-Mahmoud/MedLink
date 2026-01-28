using AutoMapper;
using MedLink.Application.DTOs.Doctors;
using MedLink.Application.Interfaces.Services;
using MedLink.Application.Specifications.Doctors;
using MedLink.Domain.Entities.Medical;
using MedLink_Application.DTOs.Doctors;
using MedLink_Application.Interfaces.Persistence;

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
    }
}
