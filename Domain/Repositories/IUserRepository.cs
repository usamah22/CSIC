namespace Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User> GetByEmailAsync(string email);
    Task<IReadOnlyList<User>> GetUsersByRoleAsync(UserRole role);
    Task<bool> ExistsAsync(string email);
    Task<User> GetUserWithBookingsAsync(Guid userId);
    Task<IReadOnlyList<User>> GetActiveUsersAsync();
}