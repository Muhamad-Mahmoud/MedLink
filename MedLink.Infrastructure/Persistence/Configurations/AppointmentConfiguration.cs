using MedLink.Domain.Entities.Appointments;
using MedLink.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedLink.Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.UserId)
            .IsRequired();

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(a => a.DoctorId)
            .IsRequired();

        builder.Property(a => a.AvailabilityId)
            .IsRequired();

        builder.Property(a => a.Status)
            .IsRequired();

        builder.Property(a => a.Fee)
            .HasPrecision(18, 2);

        builder.Property(a => a.Notes)
            .HasMaxLength(1000);

        builder.Property(a => a.CancelledReason)
            .HasMaxLength(500);

        builder.HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Availability)
            .WithOne()
            .HasForeignKey<Appointment>(a => a.AvailabilityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
