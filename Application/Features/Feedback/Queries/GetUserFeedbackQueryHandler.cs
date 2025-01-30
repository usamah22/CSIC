using Application.Common.Interfaces;
using Application.DTOs;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Feedback.Queries;

public class GetUserFeedbackQueryHandler : IRequestHandler<GetUserFeedbackQuery, List<FeedbackDto>>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly ILogger<GetUserFeedbackQueryHandler> _logger;

    public GetUserFeedbackQueryHandler(
        IFeedbackRepository feedbackRepository,
        ICurrentUserService currentUserService,
        IMapper mapper,
        ILogger<GetUserFeedbackQueryHandler> logger)
    {
        _feedbackRepository = feedbackRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<List<FeedbackDto>> Handle(GetUserFeedbackQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var feedback = await _feedbackRepository.GetByUserIdAsync(_currentUserService.UserId);
            _logger.LogInformation("Retrieved {Count} feedback items for user {UserId}", 
                feedback.Count, _currentUserService.UserId);

            var mappedFeedback = _mapper.Map<List<FeedbackDto>>(feedback);
            
            // Log the first item for debugging
            if (mappedFeedback.Any())
            {
                var firstItem = mappedFeedback.First();
                _logger.LogInformation("First feedback item: EventTitle: {Title}, UserFullName: {Name}", 
                    firstItem.EventTitle, firstItem.UserFullName);
            }

            return mappedFeedback;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user feedback");
            throw;
        }
    }
}