using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs;
using Domain;
using Domain.Repositories;
using MediatR;
using AutoMapper;
using FluentValidation;

namespace Application.Features.Users.Commands
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }


    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required");

            
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email address is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required")
                .Must(BeValidRole).WithMessage("Role must be Student, Staff, Professional, or Admin");
        }

        private bool BeValidRole(string role)
        {
            return role == "Student" || role == "Staff" || role == "Professional" || role == "Admin";
        }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Parse the role string to UserRole enum
            if (!Enum.TryParse<UserRole>(request.Role, out var userRole))
            {
                throw new ArgumentException($"Invalid role: {request.Role}");
            }

            // Hash the password
            var hashedPassword = _passwordHasher.HashPassword(request.Password);

            // Create the user
            var user = User.Create(
                email: request.Email,
                role: userRole,
                firstName: request.FirstName,
                lastName: request.LastName,
                password: hashedPassword
            );

            

            // Save to repository
            await _userRepository.AddAsync(user);

            // Return the created user
            return _mapper.Map<UserDto>(user);
        }
    }
}