using Application.Features.Feedback.Commands;
using FluentValidation;

namespace Application.Features.Events.Commands;

public class UpdateFeedbackCommandValidator : AbstractValidator<UpdateFeedbackCommand>
{
    public UpdateFeedbackCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("FeedbackId is required.");

        RuleFor(v => v.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

        RuleFor(v => v.Comment)
            .MaximumLength(1000).WithMessage("Comment must not exceed 1000 characters.");
    }
}