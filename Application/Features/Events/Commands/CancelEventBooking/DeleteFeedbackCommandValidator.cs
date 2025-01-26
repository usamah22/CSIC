using Application.Features.Feedback.Commands;
using FluentValidation;

namespace Application.Features.Events.Commands;

public class DeleteFeedbackCommandValidator : AbstractValidator<DeleteFeedbackCommand>
{
    public DeleteFeedbackCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("FeedbackId is required.");
    }
}