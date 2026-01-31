using MedLink.Domain.Entities.Appointments;
using MedLink.Domain.Entities.User;
using MedLink.Domain.Enums;
using MedLink_Application.DTOs.UserProfile;
using MedLink_Application.Interfaces.Persistence;
using MedLink_Application.Interfaces.Services;
using MedLink_Application.Specifications;

public class ProfileDashboardService : IProfileDashboardService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProfileDashboardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserProfileDashboardDto> GetMyDashboardAsync(string userId)
    {
        var profile = await _unitOfWork
            .Repository<UserProfile>()
            .FindAsync(x => x.UserId == userId);

        if (profile == null)
            throw new Exception("User profile not found");

        var totalAppointments = await _unitOfWork
            .Repository<Appointment>()
            .GetCountAsync(new AppointmentsByUserSpec(userId));

        var upcomingAppointments = await _unitOfWork
            .Repository<Appointment>()
            .GetCountAsync(
                new UpcomingAppointmentsSpec(userId)
            );


        var favoriteDoctorsCount = await _unitOfWork
            .Repository<Favorite>()
            .GetCountAsync(new FavoritesByUserSpec(userId));


        return new UserProfileDashboardDto
        {
            Profile = new ProfileHeaderDto
            {
                FullName = profile.FullName,
                Email = "", // من JWT
                ImageUrl = profile.ImageUrl,
                MemberSince = profile.CreatedAt
            },

            TotalAppointments = totalAppointments,
            UpcomingAppointments = upcomingAppointments,
            FavoriteDoctorsCount = favoriteDoctorsCount
        };

    }

}
