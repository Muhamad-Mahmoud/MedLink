using MedLink.Application.Interfaces.Services;
using MedLink.Application.Mapping;
using MedLink.Application.Services;

namespace Medical_Team_B.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<ISpecializationService, SpecializationService>();
            services.AddScoped<IFavoriteService, FavoriteService>();

            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
