using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using MediatR;

namespace DriverGuide.Application.Commands;

public class CreateTestSessionHandler(ITestSessionRepository testSessionRepository, IUserRepository userRepository) : IRequestHandler<CreateTestSessionCommand, Guid>
{
    public async Task<Guid> Handle(CreateTestSessionCommand request, CancellationToken cancellationToken)
    {
        Guid? userId = null;

        if (!string.IsNullOrEmpty(request.UserId))
        {
            var user = await userRepository.GetByGuidAsync(Guid.Parse(request.UserId));
            if (user == null)
                throw new Exception("User not found");
            
            userId = user.Id;
        }

        var testSession = new TestSession
        {
            StartDate = DateTime.Now,
            UserId = userId,
            TestSessionId = Guid.NewGuid().ToString(),
        };

        await testSessionRepository.CreateAsync(testSession);

        return Guid.Parse(testSession.TestSessionId);
    }
}
