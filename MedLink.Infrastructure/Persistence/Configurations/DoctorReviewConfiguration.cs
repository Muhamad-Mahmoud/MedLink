using MedLink.Domain.Entities.User;
using MedLink.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedLink.Infrastructure.Persistence.Configurations;

public class DoctorReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(dr => dr.Id);

        builder.Property(dr => dr.DoctorId)
            .IsRequired();

        builder.Property(dr => dr.UserId)
            .IsRequired();

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(dr => dr.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(dr => dr.Rating)
            .IsRequired();

        builder.Property(dr => dr.Comment)
            .HasMaxLength(1000);

        builder.HasOne(dr => dr.Doctor)
            .WithMany(d => d.Reviews)
            .HasForeignKey(dr => dr.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
