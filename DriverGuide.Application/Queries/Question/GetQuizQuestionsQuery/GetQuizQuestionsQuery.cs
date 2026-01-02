using DriverGuide.Domain.Enums;
using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetQuizQuestionsQuery : IRequest<ICollection<Question>>
{
    public required LicenseCategory Category { get; set; }
}
