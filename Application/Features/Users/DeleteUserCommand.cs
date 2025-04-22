using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Application.Features.Users.Commands
{
    public class DeleteUserCommand : IRequest
    {
        public string UserId { get; set; }
    }

    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");
        }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteUserCommandHandler(
            IUserRepository userRepository,
            ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _currentUserService = currentUserService;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            // Convert string ID to Guid
            Guid userId;
            if (!Guid.TryParse(request.UserId, out userId))
            {
                throw new ArgumentException($"Invalid user ID format: {request.UserId}");
            }

            // Get the user
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new NotFoundException($"User with ID {request.UserId} not found","DeleteUserCommand Error");

            // Don't allow users to delete themselves
            if (_currentUserService.UserId != null)
            {
                Guid currentUserId = _currentUserService.UserId;
                    if (user.Id == currentUserId)
                    {
                        throw new ForbiddenAccessException();
                    }
            }

            // Delete the user
            await _userRepository.DeleteAsync(user);
        }
    }
}