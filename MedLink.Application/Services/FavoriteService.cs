using AutoMapper;
using MedLink.Application.DTOs.Doctors;
using MedLink.Application.Interfaces.Services;
using MedLink.Application.Specifications.Users;
using MedLink.Domain.Entities.User;
using MedLink.Domain.Entities.Medical;
using MedLink.Domain.Exceptions;
using MedLink.Application.Interfaces.Persistence;

namespace MedLink.Application.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FavoriteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddFavoriteAsync(string userId, int doctorId)
        {
            var doctorRepo = _unitOfWork.Repository<Doctor>();
            var doctor = await doctorRepo.GetByIdAsync(doctorId);
            
            if (doctor == null)
                throw new NotFoundException($"Doctor with ID {doctorId} not found.");

            var repo = _unitOfWork.Repository<Favorite>();
            var spec = new UserFavoriteDoctorsSpec(userId, doctorId);
            var existing = await repo.GetEntityWithAsync(spec);

            if (existing != null) return;

            var favorite = new Favorite { UserId = userId, DoctorId = doctorId };
            await repo.AddAsync(favorite);
            await _unitOfWork.Complete();
        }

        public async Task RemoveFavoriteAsync(string userId, int doctorId)
        {
            var repo = _unitOfWork.Repository<Favorite>();
            var spec = new UserFavoriteDoctorsSpec(userId, doctorId);
            var existing = await repo.GetEntityWithAsync(spec);

            if (existing != null)
            {
                repo.Delete(existing);
                await _unitOfWork.Complete();
            }
        }

        public async Task<IReadOnlyList<DoctorSearchResultDto>> GetUserFavoritesAsync(string userId)
        {
            var repo = _unitOfWork.Repository<Favorite>();
            var spec = new UserFavoriteDoctorsSpec(userId);
            var favorites = await repo.GetAllWithSpecAsync(spec);

            var doctors = favorites.Select(f => f.Doctor).ToList();
            return _mapper.Map<IReadOnlyList<DoctorSearchResultDto>>(doctors);
        }
    }
}
