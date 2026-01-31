using MedLink.Application.DTOs.Doctors;
using MedLink_Application.DTOs.Doctors;

namespace MedLink.Application.Interfaces.Services
{
    public interface IDoctorService
    {
        Task<IReadOnlyList<DoctorSearchResultDto>> SearchDoctorsAsync(DoctorSearchParams searchParams);
        Task<IReadOnlyList<DoctorSearchResultDto>> GetTopRatedDoctorsAsync(int count);

    }
}
