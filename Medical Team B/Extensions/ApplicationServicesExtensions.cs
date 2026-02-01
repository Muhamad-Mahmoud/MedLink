using MedLink.Application.Interfaces.Services;
using MedLink.Application.Mapping;
using MedLink.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Medical_Team_B.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<ISpecializationService, SpecializationService>();
            services.AddScoped<IFavoriteService, FavoriteService>();

            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
            //  services.AddAutoMapper(Assembly.GetExecutingAssembly());


            return services;
        }
    }
}
