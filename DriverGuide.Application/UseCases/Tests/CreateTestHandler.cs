using DriverGuide.Domain.Entities;
using DriverGuide.Domain.Interfaces.Repositories;

namespace DriverGuide.Application.UseCases
{
    public class CreateTestHandler(ITestRepository testRepo, IUserRepository userRepo)
    {
        public async Task<Guid> HandleAsync(CreateTestCommand command)
        {
            var user = await userRepo.GetByIdAsync(Guid.Parse(command.UserId!));
            if (user == null)
                throw new Exception("User not found");

            var newTest = new Test
            {
                DataOd = DateTime.Now,
                UzytkownikId = user.Id,
                Id = Guid.NewGuid().ToString(),
            };

            await testRepo.CreateAsync(newTest);

            return Guid.Parse(newTest.Id);
        }
    }
}
