using MediatR;

namespace Application.Features.Authentication.Commands;

public record ForgotPasswordCommand : IRequest<Unit>
{
    public string Email { get; init; } = string.Empty;
}