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

            // Manual AutoMapper Registration (to support latest AutoMapper version)
            var mapperConfig = new AutoMapper.MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }
    }
}
