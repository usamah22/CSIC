using Domain.Common;

namespace Domain.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T> GetByIdAsync(Guid id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}