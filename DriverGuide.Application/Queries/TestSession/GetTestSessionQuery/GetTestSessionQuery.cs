using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetTestSessionQuery : IRequest<TestSession?>
{
    public required string TestSessionId { get; set; }
}
