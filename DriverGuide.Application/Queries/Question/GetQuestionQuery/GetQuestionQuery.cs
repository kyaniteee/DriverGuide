using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetQuestionQuery : IRequest<Question?>
{
    public required int QuestionId { get; set; }
}
