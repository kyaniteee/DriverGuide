using DriverGuide.Domain.Interfaces;
using MediatR;

namespace DriverGuide.Application.Commands;

public class CompleteTestSessionHandler(ITestSessionRepository testSessionRepository) : IRequestHandler<CompleteTestSessionCommand, bool>
{
    public async Task<bool> Handle(CompleteTestSessionCommand request, CancellationToken cancellationToken)
    {
        var testSession = await testSessionRepository.GetAsync(ts => ts.TestSessionId == request.TestSessionId);
        
        if (testSession == null)
            return false;

        testSession.EndDate = DateTimeOffset.Now;
        testSession.Result = request.Result;

        await testSessionRepository.UpdateAsync(testSession);

        return true;
    }
}
