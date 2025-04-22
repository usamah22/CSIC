using Domain;
using Domain.Repositories;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.ContactMessages.Commands;

public record CreateContactMessageCommand : IRequest<Guid>
{
    public string Name { get; init; }
    public string Email { get; init; }
    public string Message { get; init; }
}

public class CreateContactMessageCommandValidator : AbstractValidator<CreateContactMessageCommand>
{
    public CreateContactMessageCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("A valid email address is required")
            .MaximumLength(320).WithMessage("Email cannot exceed 320 characters");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required")
            .MaximumLength(1000).WithMessage("Message cannot exceed 1000 characters");
    }
}


public class CreateContactMessageCommandHandler : IRequestHandler<CreateContactMessageCommand, Guid>
{
    private readonly IContactMessageRepository _contactMessageRepository;
    private readonly ILogger<CreateContactMessageCommandHandler> _logger;

    public CreateContactMessageCommandHandler(
        IContactMessageRepository contactMessageRepository,
        ILogger<CreateContactMessageCommandHandler> logger)
    {
        _contactMessageRepository = contactMessageRepository;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateContactMessageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get result of email creation
            var emailResult = Email.Create(request.Email);
            
            // Check if email creation was successful
            if (emailResult.IsFailure)
            {
                _logger.LogWarning("Invalid email format: {Email}, Error: {Error}", 
                    request.Email, emailResult.Error);
                throw new ValidationException(emailResult.Error);
            }
            
            // Extract the email from the result
            var email = emailResult.Value;
            
            var contactMessage = ContactMessage.Create(
                request.Name,
                email,
                request.Message
            );

            await _contactMessageRepository.AddAsync(contactMessage);
            
            _logger.LogInformation("Created contact message from {Name} <{Email}>", request.Name, request.Email);
            
            return contactMessage.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating contact message: {ErrorMessage}", ex.Message);
            throw;
        }
    }
}