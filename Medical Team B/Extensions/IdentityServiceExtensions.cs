using MedLink.Domain.Identity;
using MedLink.Infrastructure.Persistence.Context;
using MedLink.Infrastructure.Persistence.UnitOfWork;
using MedLink.Application.Common.JWT;
using MedLink.Application.Interfaces.Persistence;
using MedLink.Application.Interfaces.Services;
using MedLink.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Medical_Team_B.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<Jwt>(configuration.GetSection("Jwt"));

            services.AddIdentity<ApplicationUser, IdentityRole>()
         .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();



            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(O =>
                {
                    O.RequireHttpsMetadata = false;
                    O.SaveToken = false;
                    O.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JWT:Issuer"],
                        ValidAudience = configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                    };
                });

           

            return services;
        }
    }
}
