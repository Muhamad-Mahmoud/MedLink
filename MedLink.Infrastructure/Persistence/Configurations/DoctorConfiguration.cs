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


        builder.Property(d => d.SpecialtyId)
            .IsRequired();

        builder.Property(d => d.Bio)
            .HasMaxLength(2000);

        builder.Property(d => d.ImageUrl)
            .HasMaxLength(512);
            
        builder.Property(d => d.Price)
            .HasPrecision(18, 2);

        builder.Property(d => d.ClinicDetails)
            .HasMaxLength(1000);

        builder.HasMany(d => d.Availabilities)
            .WithOne(av => av.Doctor)
            .HasForeignKey(av => av.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.Availabilities)
            .WithOne(av => av.Doctor)
            .HasForeignKey(av => av.DoctorId);

        builder.HasMany(d => d.Reviews)
            .WithOne(r => r.Doctor)
            .HasForeignKey(r => r.DoctorId);
    }
}
