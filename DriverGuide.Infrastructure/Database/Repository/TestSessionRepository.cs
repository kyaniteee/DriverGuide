using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;

namespace DriverGuide.Infrastructure.Database;

public class TestSessionRepository : RepositoryBase<TestSession>, ITestSessionRepository
{
    public TestSessionRepository(DriverGuideDbContext context) : base(context) { }

}
