using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Commands;

public class CreateTestSessionHandler(ITestSessionRepository testSessionRepository) : IRequestHandler<CreateTestSessionCommand, Guid>
{
    public async Task<Guid> Handle(CreateTestSessionCommand request, CancellationToken cancellationToken)
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
