using MedLink.Domain.Entities.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedLink.Infrastructure.Persistence.Configurations;

public class FAQConfiguration : IEntityTypeConfiguration<FAQ>
{
    public void Configure(EntityTypeBuilder<FAQ> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Question)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(f => f.Answer)
            .IsRequired()
            .HasMaxLength(2048);
    }
}
