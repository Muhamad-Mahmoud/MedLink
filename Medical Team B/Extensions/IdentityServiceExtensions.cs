using MedLink.Infrastructure.Identity;
using MedLink.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;

namespace Medical_Team_B.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
            return services;
        }
    }
}
