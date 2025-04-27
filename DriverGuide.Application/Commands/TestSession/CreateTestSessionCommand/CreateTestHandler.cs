using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;

namespace DriverGuide.Application.Commands;

public class CreateTestHandler(ITestSessionRepository testRepository, IUserRepository userRepository)
{
    public async Task<Guid> HandleAsync(CreateTestCommand command)
    {
        var user = await userRepository.GetByIdAsync(Guid.Parse(command.UserId!));
        if (user == null)
            throw new Exception("User not found");

        var newTest = new TestSession
        {
            StartDate = DateTime.Now,
            UserId = user.Id,
            TestSessionId = Guid.NewGuid().ToString(),
        };

        await testRepository.CreateAsync(newTest);

        return Guid.Parse(newTest.TestSessionId);
    }
}
