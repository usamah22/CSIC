using Application.Common.Interfaces;
using Application.Features.Authentication.Common;
using Domain;
using Domain.Exceptions;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Authentication.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthenticationResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Check if user exists
        if (await _userRepository.ExistsAsync(request.Email))
        {
            throw new DomainException("User with this email already exists.");
        }

        // Create user
        var user = User.Create(
            request.Email,
            UserRole.Student,
            request.FirstName,
            request.LastName,
            _passwordHasher.HashPassword(request.Password)
            
        );

        // Save user
        await _userRepository.AddAsync(user);

        // Generate JWT token
        var token = _jwtTokenGenerator.GenerateToken(user);

        // Return authentication response
        return new AuthenticationResponse
        {
            Token = token,
            RefreshToken = string.Empty, // Implement refresh token if needed
            ExpiresAt = DateTime.UtcNow.AddDays(7) // Adjust expiration as needed
        };
    }
}