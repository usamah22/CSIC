using MediatR;

namespace Application.Features.Authentication.Commands;

public record ResetPasswordCommand : IRequest<Unit>
{
    public string Email { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
    public string NewPassword { get; init; } = string.Empty;
}