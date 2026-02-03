using AutoMapper;
using MedLink.Application.DTOs.Doctors;
using MedLink.Application.Interfaces.Persistence;
using MedLink.Application.Interfaces.Services;
using MedLink.Application.Interfaces.Specifications;
using MedLink.Application.Specifications.Medical;
using MedLink.Domain.Entities.Medical;
using MedLink.Domain.Exceptions;

namespace MedLink.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IGenericRepository<Doctor> _doctorRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public DoctorService(IGenericRepository<Doctor> doctorRepo, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _doctorRepo = doctorRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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

        public async Task<Doctor> AddDoctorAsync(Doctor doctor)
        {
            var repo = _unitOfWork.Repository<Doctor>();
            await repo.AddAsync(doctor);
            await _unitOfWork.Complete();
            return doctor;
        }


        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            var repo = _unitOfWork.Repository<Doctor>();
            repo.Update(doctor);
            await _unitOfWork.Complete();
        }


        public async Task DeleteDoctorAsync(int id)
        {
            var repo = _unitOfWork.Repository<Doctor>();


            var doctor = await repo.GetByIdAsync(id);

            if (doctor != null)
            {
                doctor.IsDeleted = true;
                await _unitOfWork.Complete();
            }
            else
            {
                throw new NotFoundException($"Doctor with ID {id} not found.");
            }
        }

        public async Task<Doctor?> GetDoctorByIdAsync(int id)
        {
            var repo = _unitOfWork.Repository<Doctor>();
            return await repo.GetByIdAsync(id);
        }

        public async Task<IReadOnlyList<Doctor>> GetAllDoctorsAsync(ISpecification<Doctor>? spec = null)
        {


            var repo = _unitOfWork.Repository<Doctor>();
            return spec != null
                ? await repo.GetAllWithSpecAsync(spec)
                : await repo.GetAllAsync();
        }






    }
}

