using DriverGuide.Domain.Interfaces;
using MediatR;

namespace DriverGuide.Application.Requests;

public class CompleteTestSessionHandler(ITestSessionRepository testSessionRepository) : IRequestHandler<CompleteTestSessionRequest, Unit>
{
    public async Task<Unit> Handle(CompleteTestSessionRequest request, CancellationToken cancellationToken)
    {
        var testSession = await testSessionRepository.GetAsync(ts => ts.TestSessionId == request.TestSessionId)
            ?? throw new InvalidOperationException($"Test session {request.TestSessionId} not found");

        testSession.EndDate = DateTimeOffset.Now;
        testSession.Result = request.Result;

        await testSessionRepository.UpdateAsync(testSession);

        return Unit.Value;
    }
}
