using MedLink.Application.Interfaces.Services;
using MedLink.Application.Mapping;
using MedLink.Application.Services;
using MedLink.Infrastructure.Services;
using MedLink_Application.Interfaces.Services;
using MedLink_Application.Services;

namespace Medical_Team_B.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<ISpecializationService, SpecializationService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<IProfileDashboardService, ProfileDashboardService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IUserLanguageService, UserLanguageService>();
            services.AddScoped<IProfileAppointmentService, ProfileAppointmentService>();
            services.AddScoped<IImageService, ImageService>();

            services.AddSingleton<ILanguageService, LanguageService>();

            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
