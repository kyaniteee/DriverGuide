using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetQuestionFilesByNamesQuery : IRequest<List<QuestionFile>>
{
    public required List<string> QuestionFileNames { get; set; }
}
