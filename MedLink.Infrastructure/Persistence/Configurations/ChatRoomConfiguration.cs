using MedLink.Domain.Entities.Chat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedLink.Infrastructure.Persistence.Configurations;

public class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoom>
{
    public void Configure(EntityTypeBuilder<ChatRoom> builder)
    {
        builder.HasKey(cr => cr.Id);

        builder.Property(cr => cr.AppointmentId)
            .IsRequired();

        builder.HasOne(cr => cr.Appointment)
            .WithOne()
            .HasForeignKey<ChatRoom>(cr => cr.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
