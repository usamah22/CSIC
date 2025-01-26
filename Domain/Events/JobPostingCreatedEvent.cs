using Domain.Common;

namespace Domain.Events;

public class JobPostingCreatedEvent : DomainEvent
{
    public Guid JobPostingId { get; }
    public string Title { get; }
    public DateTime ClosingDate { get; }

    public JobPostingCreatedEvent(Guid jobPostingId, string title, DateTime closingDate)
    {
        JobPostingId = jobPostingId;
        Title = title;
        ClosingDate = closingDate;
    }
}