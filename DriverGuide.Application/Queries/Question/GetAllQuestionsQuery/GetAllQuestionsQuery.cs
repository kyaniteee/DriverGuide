using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetAllQuestionsQuery : IRequest<ICollection<Question>>
{
}
