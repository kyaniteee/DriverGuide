using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetAllQuestionFilesQuery : IRequest<ICollection<QuestionFile>>
{
}
