using MedLink.Domain.Entities.Medical;
using MedLink.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedLink.Infrastructure.Persistence.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.UserId)
            .IsRequired();

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(d => d.SpecializationId)
            .IsRequired();

        builder.Property(d => d.Bio)
            .HasMaxLength(2000);

        builder.Property(d => d.ImageUrl)
            .HasMaxLength(512);

        builder.Property(d => d.ClinicDetails)
            .HasMaxLength(1000);

        builder.HasOne(d => d.Specialization)
            .WithMany(s => s.Doctors)
            .HasForeignKey(d => d.SpecializationId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasMany(d => d.Appointments)
            .WithOne(a => a.Doctor)
            .HasForeignKey(a => a.DoctorId);

        builder.HasMany(d => d.Availabilities)
            .WithOne(av => av.Doctor)
            .HasForeignKey(av => av.DoctorId);

        builder.HasMany(d => d.Reviews)
            .WithOne(r => r.Doctor)
            .HasForeignKey(r => r.DoctorId);
    }
}
