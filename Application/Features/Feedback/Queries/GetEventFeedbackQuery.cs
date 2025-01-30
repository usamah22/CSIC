using Application.DTOs;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Feedback.Queries;

public record GetEventFeedbackQuery(Guid EventId) : IRequest<List<FeedbackDto>>;

public class GetEventFeedbackQueryHandler : IRequestHandler<GetEventFeedbackQuery, List<FeedbackDto>>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IMapper _mapper;

    public GetEventFeedbackQueryHandler(IFeedbackRepository feedbackRepository, IMapper mapper)
    {
        _feedbackRepository = feedbackRepository;
        _mapper = mapper;
    }

    public async Task<List<FeedbackDto>> Handle(GetEventFeedbackQuery request, CancellationToken cancellationToken)
    {
        var feedback = await _feedbackRepository.GetEventFeedbackAsync(request.EventId);
        return _mapper.Map<List<FeedbackDto>>(feedback);
    }
}