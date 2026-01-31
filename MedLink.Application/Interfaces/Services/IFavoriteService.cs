using MedLink.Application.DTOs.Doctors;

namespace MedLink.Application.Interfaces.Services
{
    public interface IFavoriteService
    {
        Task AddFavoriteAsync(string userId, int doctorId);
        Task RemoveFavoriteAsync(string userId, int doctorId);
        Task<IReadOnlyList<DoctorSearchResultDto>> GetUserFavoritesAsync(string userId);
    }
}
