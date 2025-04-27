using DriverGuide.Domain.Models;
using DriverGuide.Domain.Interfaces;
using System.Linq.Expressions;

namespace DriverGuide.Infrastructure.Database;
public class TestSqlRepository : RepositoryBase, ITestSessionRepository
{
    public TestSqlRepository(DriverGuideDbContext context) : base(context) { }

    public Task<TestSession> CreateAsync(TestSession dbRecord)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(TestSession dbRecord)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<TestSession>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TestSession> GetAsync(Expression<Func<TestSession, bool>> filter, bool useNoTracking = false)
    {
        throw new NotImplementedException();
    }

    public Task<TestSession> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<TestSession> UpdateAsync(TestSession dbRecord)
    {
        throw new NotImplementedException();
    }
}
