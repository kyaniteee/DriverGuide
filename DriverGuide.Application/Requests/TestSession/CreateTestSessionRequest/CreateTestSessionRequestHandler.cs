using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Requests;

public class CreateTestSessionRequestHandler(ITestSessionRepository testSessionRepository) : IRequestHandler<CreateTestSessionRequest, Guid>
{
    public async Task<Guid> Handle(CreateTestSessionRequest request, CancellationToken cancellationToken)
    {
        var testSession = new TestSession
        {
            TestSessionId = Guid.NewGuid().ToString(),
            StartDate = request.StartDate,
            UserId = request.UserId,
            EndDate = null,
            Result = null
        };

        await testSessionRepository.CreateAsync(testSession);

        return Guid.Parse(testSession.TestSessionId);
    }
}
