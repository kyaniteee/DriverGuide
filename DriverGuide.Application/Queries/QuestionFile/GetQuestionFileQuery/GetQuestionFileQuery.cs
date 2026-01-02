using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetQuestionFileQuery : IRequest<QuestionFile?>
{
    public required int QuestionFileId { get; set; }
}
