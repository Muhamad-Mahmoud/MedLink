using MedLink.Domain.Entities.Appointments;
using MedLink.Domain.Entities.Medical;
using MedLink.Domain.Enums;
using MedLink.Domain.Identity;
using MedLink.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace MedLink.Infrastructure.Persistence.Seed
{
    public static class ApplicationDbContextSeed
    {
        // SRID 4326 = WGS84 (standard GPS coordinate system)
        private const int Srid = 4326;

        public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles
            var adminRoleId = "d11126cb-d069-4e04-a165-d4cf495d513d";
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var role = new IdentityRole("Admin")
                {
                    Id = adminRoleId
                };
                await roleManager.CreateAsync(role);
            }

            // Seed Admin User
            var adminEmail = "admin@medlink.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Admin",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Seed Specializations
            if (!await context.Specializations.AnyAsync())
            {
                var specializations = new List<Specialization>
                {
                    new Specialization { Name = "Cardiology", Description = "Heart Specialist" },
                    new Specialization { Name = "Dermatology", Description = "Skin Specialist" },
                    new Specialization { Name = "Pediatrics", Description = "Child Specialist" },
                    new Specialization { Name = "Orthopedics", Description = "Bone Specialist" },
                    new Specialization { Name = "Neurology", Description = "Brain Specialist" }
                };

                await context.Specializations.AddRangeAsync(specializations);
                await context.SaveChangesAsync();
            }

            // Seed Doctors with Location and Availability
            if (!await context.Doctors.AnyAsync())
            {
                // Retrieve seeds to assign relations properly
                var cardio = await context.Specializations.FirstAsync(s => s.Name == "Cardiology");
                var derma = await context.Specializations.FirstAsync(s => s.Name == "Dermatology");
                var pedia = await context.Specializations.FirstAsync(s => s.Name == "Pediatrics");

                var doctors = new List<Doctor>
                {
                    new Doctor
                    {
                        Name = "Dr. Ahmed Ali",
                        SpecialtyId = cardio.Id,
                        City = "Cairo",
                        Bio = "Expert Cardiologist with over 15 years of experience in treating complex heart conditions.",
                        Price = 500,
                        Location = CreatePoint(31.2357, 30.0444), // Downtown Cairo (Lng, Lat)
                        Gender = Gender.Male,
                        ImageUrl = "https://randomuser.me/api/portraits/men/32.jpg",
                        Address = "123 Tahrir St, Downtown, Cairo",
                        Availabilities = new List<DoctorAvailability>
                        {
                            new DoctorAvailability { Date = DateTime.Today, StartTime = new TimeSpan(10,0,0), EndTime = new TimeSpan(14,0,0) },
                            new DoctorAvailability { Date = DateTime.Today.AddDays(1), StartTime = new TimeSpan(10,0,0), EndTime = new TimeSpan(14,0,0) }
                        }
                    },
                    new Doctor
                    {
                        Name = "Dr. Mona Sayed",
                        SpecialtyId = derma.Id,
                        City = "Giza",
                        Bio = "Certified Dermatologist specializing in cosmetic procedures and skin health.",
                        Price = 300,
                        Location = CreatePoint(31.2089, 30.0131), // Dokki, Giza area
                        Gender = Gender.Female,
                        ImageUrl = "https://randomuser.me/api/portraits/women/44.jpg",
                        Address = "45 Dokki St, Giza",
                        Availabilities = new List<DoctorAvailability>
                        {
                            new DoctorAvailability { Date = DateTime.Today, StartTime = new TimeSpan(16,0,0), EndTime = new TimeSpan(20,0,0) }
                        }
                    },
                    new Doctor
                    {
                        Name = "Dr. Khaled Ibrahim",
                        SpecialtyId = pedia.Id,
                        City = "Alexandria",
                        Bio = "Friendly Pediatrician dedicated to the health and well-being of children.",
                        Price = 250,
                        Location = CreatePoint(29.9187, 31.2001), // Alexandria
                        Gender = Gender.Male,
                        ImageUrl = "https://randomuser.me/api/portraits/men/85.jpg",
                        Address = "10 Corniche Rd, Alexandria",
                        Availabilities = new List<DoctorAvailability>
                        {
                            new DoctorAvailability { Date = DateTime.Today.AddDays(2), StartTime = new TimeSpan(9,0,0), EndTime = new TimeSpan(12,0,0) }
                        }
                    },
                     new Doctor
                    {
                        Name = "Dr. Sarah Nabil",
                        SpecialtyId = cardio.Id,
                        City = "Cairo",
                        Bio = "Cardiology consultant focusing on preventive care.",
                        Price = 450,
                        Location = CreatePoint(31.3283, 30.0875), // Heliopolis, Cairo
                        Gender = Gender.Female,
                        ImageUrl = "https://randomuser.me/api/portraits/women/65.jpg",
                        Address = "99 Merghany St, Heliopolis, Cairo",
                        Availabilities = new List<DoctorAvailability>
                        {
                            new DoctorAvailability { Date = DateTime.Today.AddDays(1), StartTime = new TimeSpan(12,0,0), EndTime = new TimeSpan(16,0,0) }
                        }
                    }
                };

                await context.Doctors.AddRangeAsync(doctors);
                await context.SaveChangesAsync();
            }
        }

        private static Point CreatePoint(double longitude, double latitude)
        {
            return new Point(longitude, latitude) { SRID = Srid };
        }
    }
}

