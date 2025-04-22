using Domain;

namespace Domain.Repositories;

public interface IContactMessageRepository : IBaseRepository<ContactMessage>
{
    Task<IReadOnlyList<ContactMessage>> GetUnreadMessagesAsync();
    Task<int> GetUnreadCountAsync();
    Task MarkAsReadAsync(Guid id);
    Task MarkAsUnreadAsync(Guid id);
}