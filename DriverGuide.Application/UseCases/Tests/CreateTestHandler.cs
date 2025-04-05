using DriverGuide.Domain.Entities;
using DriverGuide.Domain.Interfaces.Repositories;

namespace DriverGuide.Application.UseCases.Tests
{
    public class CreateTestHandler
    {
        private readonly ITestRepository _testRepository;
        private readonly IUserRepository _userRepository;

        public CreateTestHandler(ITestRepository testRepo, IUserRepository userRepo)
        {
            _testRepository = testRepo;
            _userRepository = userRepo;
        }

        public async Task<Guid> HandleAsync(CreateTestCommand command)
        {
            var user = await _userRepository.GetByIdAsync(Guid.Parse(command.UserId));
            if (user == null)
                throw new Exception("User not found");

            var newTest = new Test
            {
                DataOd = DateTime.Now,
                UzytkownikId = user.Id,
                Id = Guid.NewGuid().ToString(),
            };

            await _testRepository.CreateAsync(newTest);

            return Guid.Parse(newTest.Id);
        }
    }
}
