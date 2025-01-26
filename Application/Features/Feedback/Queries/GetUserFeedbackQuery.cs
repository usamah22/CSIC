using Application.DTOs;
using MediatR;

namespace Application.Features.Feedback.Queries;

public record GetUserFeedbackQuery : IRequest<List<FeedbackDto>>;