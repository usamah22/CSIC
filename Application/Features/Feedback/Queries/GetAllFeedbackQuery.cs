using Application.DTOs;
using MediatR;

namespace Application.Features.Feedback.Queries;

public record GetAllFeedbackQuery : IRequest<List<FeedbackDto>>;
