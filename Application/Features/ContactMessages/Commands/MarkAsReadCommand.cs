using Domain;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.ContactMessages.Commands;

public record MarkAsReadCommand : IRequest<Unit>
{
    public Guid Id { get; init; }
}

public class MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommand, Unit>
{
    private readonly IContactMessageRepository _contactMessageRepository;
    private readonly ILogger<MarkAsReadCommandHandler> _logger;

    public MarkAsReadCommandHandler(
        IContactMessageRepository contactMessageRepository,
        ILogger<MarkAsReadCommandHandler> logger)
    {
        _contactMessageRepository = contactMessageRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var message = await _contactMessageRepository.GetByIdAsync(request.Id);
            
            if (message == null)
            {
                _logger.LogWarning("Contact message with ID {MessageId} not found", request.Id);
                throw new NotFoundException(nameof(ContactMessage), request.Id);
            }

            await _contactMessageRepository.MarkAsReadAsync(request.Id);
            
            return Unit.Value;
        }
        catch (Exception ex) when (!(ex is NotFoundException))
        {
            _logger.LogError(ex, "Error marking contact message {MessageId} as read: {ErrorMessage}", 
                request.Id, ex.Message);
            throw;
        }
    }
}