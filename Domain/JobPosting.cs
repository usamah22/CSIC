using Domain.Common;

namespace Domain;

public class JobPosting : BaseEntity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string CompanyName { get; private set; }
    public string Location { get; private set; }
    public DateTime ClosingDate { get; private set; }
    public string ExternalApplicationUrl { get; private set; }
    public JobType Type { get; private set; }
    public bool IsActive { get; private set; }
    public Guid CreatedById { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastModifiedAt { get; private set; }

    private JobPosting() { } // For EF Core

    public static JobPosting Create(
        string title,
        string description,
        string companyName,
        string location,
        DateTime closingDate,
        string externalApplicationUrl,
        JobType type,
        Guid createdById)
    {
        return new JobPosting
        {
            Title = title,
            Description = description,
            CompanyName = companyName,
            Location = location,
            ClosingDate = closingDate,
            ExternalApplicationUrl = externalApplicationUrl,
            Type = type,
            IsActive = true,
            CreatedById = createdById,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Deactivate()
    {
        IsActive = false;
        LastModifiedAt = DateTime.UtcNow;
    }
}

public enum JobType
{
    Placement,
    Graduate,
    PartTime
}