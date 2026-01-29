using MedLink.Domain.Entities.Appointments;
using MedLink.Domain.Entities.Medical;
using MedLink.Domain.Enums;
using MedLink.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MedLink.Infrastructure.Persistence.Seed
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
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
                        Latitude = 30.0444,
                        Longitude = 31.2357, // Downtown Cairo
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
                        Latitude = 30.0131,
                        Longitude = 31.2089, // Dokki, Giza area
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
                        Latitude = 31.2001,
                        Longitude = 29.9187, // Alexandria
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
                        Latitude = 30.0875,
                        Longitude = 31.3283, // Heliopolis, Cairo
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
    }
}
