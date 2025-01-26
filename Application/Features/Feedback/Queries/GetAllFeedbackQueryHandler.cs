using Application.DTOs;
using AutoMapper;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Feedback.Queries;

public class GetAllFeedbackQueryHandler : IRequestHandler<GetAllFeedbackQuery, List<FeedbackDto>>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IMapper _mapper;

    public GetAllFeedbackQueryHandler(
        IFeedbackRepository feedbackRepository,
        IMapper mapper)
    {
        _feedbackRepository = feedbackRepository;
        _mapper = mapper;
    }

    public async Task<List<FeedbackDto>> Handle(GetAllFeedbackQuery request, CancellationToken cancellationToken)
    {
        var feedback = await _feedbackRepository.GetAllAsync();
        return _mapper.Map<List<FeedbackDto>>(feedback);
    }
}