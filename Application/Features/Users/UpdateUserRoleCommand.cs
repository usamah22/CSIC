using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain;
using Domain.Repositories;
using MediatR;
using FluentValidation;
using Application.Common.Exceptions;

namespace Application.Features.Users.Commands
{
    public class UpdateUserRoleCommand : IRequest
    {
        public string UserId { get; set; }
        public string Role { get; set; }
    }

    public class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
    {
        public UpdateUserRoleCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required")
                .Must(BeValidRole).WithMessage("Role must be Student, Staff, Professional, or Admin");
        }

        private bool BeValidRole(string role)
        {
            return role == "Student" || role == "Staff" || role == "Professional" || role == "Admin";
        }
    }

    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateUserRoleCommandHandler(
            IUserRepository userRepository,
            ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
        }

        public async Task Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            // Convert string ID to Guid
            Guid userId;
            if (!Guid.TryParse(request.UserId, out userId))
            {
                throw new ArgumentException($"Invalid user ID format: {request.UserId}");
            }

            // Get the user
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new NotFoundException($"User with ID {request.UserId} not found", "UpdateUserRoleCommand.Error");

            // Don't allow users to change their own role
            if (_currentUserService.UserId != null)
            {
                Guid currentUserId = _currentUserService.UserId;

                    if (user.Id == currentUserId)
                    {
                        throw new ForbiddenAccessException();
                    }

            }

            // Parse the role string to UserRole enum
            UserRole userRole;
            if (!Enum.TryParse(request.Role, out userRole))
            {
                throw new ArgumentException($"Invalid role: {request.Role}");
            }

            // Update the role using the domain method
            user.UpdateRole(userRole);


            await _userRepository.UpdateAsync(user);
        }
    }
}