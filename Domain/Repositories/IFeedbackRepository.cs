namespace Domain.Repositories;

public interface IFeedbackRepository : IBaseRepository<Feedback>
{
    Task<IReadOnlyList<Feedback>> GetByUserIdAsync(Guid userId);
    Task<IReadOnlyList<Feedback>> GetEventFeedbackAsync(Guid eventId);
    Task<Feedback?> GetUserEventFeedbackAsync(Guid eventId, Guid userId);
}