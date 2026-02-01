using MedLink.Application.Mapping;
using MedLink.Infrastructure.Persistence.Context;
using MedLink.Infrastructure.Persistence.Repositories;
using MedLink.Infrastructure.Persistence.UnitOfWork;
using MedLink.Application.Interfaces.Persistence;
using MedLink.Application.Interfaces.Services;
using MedLink.Application.Mapping;
using MedLink.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using MedLink_Application.Interfaces.Persistence;
using MedLink_Application.Interfaces.Services;
using MedLink_Application.Mapping;
using MedLink_Application.Services;
using Microsoft.EntityFrameworkCore;

namespace Medical_Team_B.Extensions
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection String Not Found.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString,
                    b => 
                    {
                        b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        b.UseNetTopologySuite(); // Enable spatial data support
                    }));

            //services.AddAutoMapper(typeof(AuthMappingProfiles));
            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

            services.AddScoped<EmailToUsernameResolver>();


            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IAuthService), typeof(AuthService));


            return services;
        }
    }
}
