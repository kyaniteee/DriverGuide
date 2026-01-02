using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetUserTestSessionsQuery : IRequest<List<TestSession>>
{
    public required Guid UserId { get; set; }
}
