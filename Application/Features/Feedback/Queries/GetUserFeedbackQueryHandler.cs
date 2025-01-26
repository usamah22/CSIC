using Application.Common.Interfaces;
using Application.DTOs;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Feedback.Queries;

public class GetUserFeedbackQueryHandler : IRequestHandler<GetUserFeedbackQuery, List<FeedbackDto>>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetUserFeedbackQueryHandler(
        IFeedbackRepository feedbackRepository,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _feedbackRepository = feedbackRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<List<FeedbackDto>> Handle(GetUserFeedbackQuery request, CancellationToken cancellationToken)
    {
        var feedback = await _feedbackRepository.GetByUserIdAsync(_currentUserService.UserId);
        return _mapper.Map<List<FeedbackDto>>(feedback);
    }
}