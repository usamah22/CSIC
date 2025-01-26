using MediatR;

namespace Application.Features.Authentication.Commands;

public record ChangePasswordCommand : IRequest<Unit>
{
    public string CurrentPassword { get; init; } = string.Empty;
    public string NewPassword { get; init; } = string.Empty;
}