namespace Domain.Repositories;

public interface IJobPostingRepository : IBaseRepository<JobPosting>
{
    Task<IReadOnlyList<JobPosting>> GetActiveJobPostingsAsync();
    Task<IReadOnlyList<JobPosting>> GetJobPostingsByTypeAsync(JobType type);
    Task<IReadOnlyList<JobPosting>> GetRecentJobPostingsAsync(int count);
    Task<IReadOnlyList<JobPosting>> SearchJobPostingsAsync(string searchTerm);
    Task<IReadOnlyList<JobPosting>> GetJobPostingsByCompanyAsync(string companyName);
    Task<bool> IsCompanyLimitReachedAsync(string companyName, int maxPostingsPerCompany);
    Task DeactivateExpiredJobPostingsAsync();
}