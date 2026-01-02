using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetQuestionFileByNameQuery : IRequest<QuestionFile?>
{
    public required string QuestionFileName { get; set; }
}
