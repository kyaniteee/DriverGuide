using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Queries;

public class GetTestSessionHandler : IRequestHandler<GetTestSessionQuery, TestSession?>
{
    private readonly ITestSessionRepository _repository;

    public GetTestSessionHandler(ITestSessionRepository repository)
    {
        _repository = repository;
    }

    public async Task<TestSession?> Handle(GetTestSessionQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.TestSessionId);
    }
}
