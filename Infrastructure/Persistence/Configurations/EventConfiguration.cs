using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.Property(e => e.Id)
            .HasColumnType("uuid")
            .HasDefaultValueSql("uuid_generate_v4()");

        builder.Property(e => e.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnType("text");

        builder.Property(e => e.Location)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasMany(e => e.Bookings)
            .WithOne(b => b.Event)
            .HasForeignKey(b => b.EventId)
            .OnDelete(DeleteBehavior.Cascade);
            
        // Add index for common queries
        builder.HasIndex(e => e.StartDate);
        builder.HasIndex(e => e.Status);
    }
}