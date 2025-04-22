using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ContactMessagesConfiguration : IEntityTypeConfiguration<ContactMessage>
{
    public void Configure(EntityTypeBuilder<ContactMessage> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Message).IsRequired().HasMaxLength(1000);
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.IsRead).IsRequired();
        
        // Handle complex type Email as an owned entity
        builder.OwnsOne(e => e.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(320);
        });
    }
}