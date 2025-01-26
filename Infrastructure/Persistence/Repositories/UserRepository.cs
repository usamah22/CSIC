using Domain;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IReadOnlyList<User>> GetUsersByRoleAsync(UserRole role)
    {
        return await _context.Users
            .Where(u => u.Role == role && u.IsActive)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task<User> GetUserWithBookingsAsync(Guid userId)
    {
        return await _context.Users
            .Include(u => u.EventBookings)
            .ThenInclude(b => b.Event)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<IReadOnlyList<User>> GetActiveUsersAsync()
    {
        return await _context.Users
            .Where(u => u.IsActive)
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .ToListAsync();
    }
}