using Domain.Common;

namespace Domain;

public class User : BaseEntity
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string FirstName { get; private set; } = "";
    public string LastName { get; private set; } = "";
    public string Password { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastModifiedAt { get; private set; }
    
    private readonly List<EventBooking> _eventBookings = new();
    public IReadOnlyCollection<EventBooking> EventBookings => _eventBookings.AsReadOnly();
    
    private readonly List<Feedback> _feedback = new();
    public IReadOnlyCollection<Feedback> Feedback => _feedback.AsReadOnly();

    public string FullName => string.IsNullOrWhiteSpace($"{FirstName} {LastName}".Trim()) 
        ? Email 
        : $"{FirstName} {LastName}".Trim();
    
    private User() { } // For EF Core

    public static User Create(
        string email,
        UserRole role,
        string firstName = "",
        string lastName = "",
        string password = ""
    )
    {
        return new User
        {
            Id = Guid.NewGuid(),   
            Email = email.ToLower(),
            FirstName = firstName,
            LastName = lastName,
            Password = password,
            Role = role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow  
        };
    }

    
    public void UpdateRole(UserRole newRole)
    {
        Role = newRole;
        LastModifiedAt = DateTime.UtcNow;
    }
    
    public void SetActive(bool isActive)
    {
        IsActive = isActive;
        LastModifiedAt = DateTime.UtcNow;
    }
}

public enum UserRole
{
    Student,
    Staff,
    Professional,
    Admin
}