using Application.Common.Interfaces;
using Domain.Repositories;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => 
                {
                    b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    b.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                }));

        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJobPostingRepository, JobPostingRepository>();
        services.AddScoped<IFeedbackRepository, FeedbackRepository>();

        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}