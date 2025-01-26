using Application.Common.Interfaces;
using Application.Features.Authentication.Common;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Authentication.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Validate user exists
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials");

        // Validate password
        if (!_passwordHasher.VerifyPassword(request.Password, user.Password))
            throw new UnauthorizedAccessException("Invalid credentials");

        // Generate JWT token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResponse
        {
            Token = token,
            RefreshToken = string.Empty, // Implement if needed
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
    }
}