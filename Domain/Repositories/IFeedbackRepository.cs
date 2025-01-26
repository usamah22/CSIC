namespace Domain.Repositories;

public interface IFeedbackRepository : IBaseRepository<Feedback>
{
    Task<IReadOnlyList<Feedback>> GetByUserIdAsync(Guid userId);
}