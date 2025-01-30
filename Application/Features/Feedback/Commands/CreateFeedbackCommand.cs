using FluentValidation;
using MediatR;

namespace Application.Features.Feedback.Commands;

public record CreateFeedbackCommand : IRequest<Guid>
{
    public Guid EventId { get; init; }
    public int Rating { get; init; }
    public string Comment { get; init; } = string.Empty;
}

public class CreateFeedbackCommandValidator : AbstractValidator<CreateFeedbackCommand>
{
    public CreateFeedbackCommandValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty();

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating must be between 1 and 5");

        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage("Comment cannot be empty")
            .MaximumLength(1000)
            .WithMessage("Comment cannot be longer than 1000 characters");
    }
}