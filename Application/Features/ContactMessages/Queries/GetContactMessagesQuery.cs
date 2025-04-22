using Application.DTOs;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.ContactMessages.Queries;

public record GetContactMessagesQuery : IRequest<List<ContactMessageDto>>
{
    public bool UnreadOnly { get; init; } = false;
}

public class GetContactMessagesQueryHandler : IRequestHandler<GetContactMessagesQuery, List<ContactMessageDto>>
{
    private readonly IContactMessageRepository _contactMessageRepository;
    private readonly ILogger<GetContactMessagesQueryHandler> _logger;

    public GetContactMessagesQueryHandler(
        IContactMessageRepository contactMessageRepository,
        ILogger<GetContactMessagesQueryHandler> logger)
    {
        _contactMessageRepository = contactMessageRepository;
        _logger = logger;
    }

    public async Task<List<ContactMessageDto>> Handle(GetContactMessagesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var messages = request.UnreadOnly
                ? await _contactMessageRepository.GetUnreadMessagesAsync()
                : await _contactMessageRepository.GetAllAsync();

            _logger.LogInformation("Retrieved {Count} contact messages", messages.Count);

            return messages.Select(m => new ContactMessageDto
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email.Value,
                Message = m.Message,
                CreatedAt = m.CreatedAt,
                IsRead = m.IsRead
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving contact messages: {ErrorMessage}", ex.Message);
            throw;
        }
    }
}