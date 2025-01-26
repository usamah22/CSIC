using Domain;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class JobPostingRepository : BaseRepository<JobPosting>, IJobPostingRepository
{
    private readonly ApplicationDbContext _context;

    public JobPostingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<JobPosting>> GetActiveJobPostingsAsync()
    {
        return await _context.JobPostings
            .Where(j => j.IsActive && j.ClosingDate > DateTime.UtcNow)
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<JobPosting>> GetJobPostingsByTypeAsync(JobType type)
    {
        return await _context.JobPostings
            .Where(j => j.Type == type && j.IsActive && j.ClosingDate > DateTime.UtcNow)
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<JobPosting>> GetRecentJobPostingsAsync(int count)
    {
        return await _context.JobPostings
            .Where(j => j.IsActive && j.ClosingDate > DateTime.UtcNow)
            .OrderByDescending(j => j.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<JobPosting>> SearchJobPostingsAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetActiveJobPostingsAsync();

        return await _context.JobPostings
            .Where(j => 
                j.IsActive && 
                j.ClosingDate > DateTime.UtcNow &&
                (EF.Functions.ILike(j.Title, $"%{searchTerm}%") ||
                 EF.Functions.ILike(j.Description, $"%{searchTerm}%") ||
                 EF.Functions.ILike(j.CompanyName, $"%{searchTerm}%")))
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<JobPosting>> GetJobPostingsByCompanyAsync(string companyName)
    {
        return await _context.JobPostings
            .Where(j => 
                j.IsActive && 
                j.ClosingDate > DateTime.UtcNow &&
                EF.Functions.ILike(j.CompanyName, companyName))
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> IsCompanyLimitReachedAsync(string companyName, int maxPostingsPerCompany)
    {
        var activePostingsCount = await _context.JobPostings
            .CountAsync(j => 
                j.IsActive && 
                j.ClosingDate > DateTime.UtcNow &&
                EF.Functions.ILike(j.CompanyName, companyName));

        return activePostingsCount >= maxPostingsPerCompany;
    }

    public async Task DeactivateExpiredJobPostingsAsync()
    {
        var expiredPostings = await _context.JobPostings
            .Where(j => j.IsActive && j.ClosingDate <= DateTime.UtcNow)
            .ToListAsync();

        foreach (var posting in expiredPostings)
        {
            posting.Deactivate();
        }

        await _context.SaveChangesAsync();
    }
}