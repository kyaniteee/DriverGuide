using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetUserTestSessionsHandler : IRequestHandler<GetUserTestSessionsQuery, List<TestSession>>
{
    private readonly ITestSessionRepository _repository;

    public GetUserTestSessionsHandler(ITestSessionRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TestSession>> Handle(GetUserTestSessionsQuery request, CancellationToken cancellationToken)
    {
        var allSessions = await _repository.FindAsync(ts => ts.UserId == request.UserId);
        return allSessions.OrderByDescending(s => s.StartDate).ToList();
    }
}
