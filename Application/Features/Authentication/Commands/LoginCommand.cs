using Application.Features.Authentication.Common;
using MediatR;

namespace Application.Features.Authentication.Commands;

public record LoginCommand : IRequest<AuthenticationResponse>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}