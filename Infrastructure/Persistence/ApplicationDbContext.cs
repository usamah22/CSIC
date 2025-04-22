using System.Reflection;
using Domain;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<EventBooking> EventBookings { get; set; }
    public DbSet<JobPosting> JobPostings { get; set; }
    public DbSet<Feedback> Feedback { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    
    public DbSet<ContactMessage> ContactMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp"); // Enable UUID extension
        
        // Use case-insensitive text search for PostgreSQL
        modelBuilder.UseCollation("und-x-icu");
        
        modelBuilder.Ignore<DomainEvent>();
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Configure PostgreSQL-specific types
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Convert all DateTime to timestamptz
            var properties = entity.GetProperties()
                .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?));
            
            foreach (var property in properties)
            {
                property.SetColumnType("timestamp with time zone");
            }
        }

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedAt = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}