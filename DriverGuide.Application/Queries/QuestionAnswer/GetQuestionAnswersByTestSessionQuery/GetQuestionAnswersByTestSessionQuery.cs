using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetQuestionAnswersByTestSessionQuery : IRequest<ICollection<QuestionAnswer>>
{
    public required string TestSessionId { get; set; }
}
