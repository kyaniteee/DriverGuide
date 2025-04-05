using DriverGuide.Domain.Entities;
using DriverGuide.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace DriverGuide.Infrastructure.Core.Repository
{
    public class TestRepository : RepositoryBase, ITestRepository
    {
        public TestRepository(AppDbContext context) : base(context) { }

        public Task<Test> CreateAsync(Test dbRecord)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Test dbRecord)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Test>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Test> GetAsync(Expression<Func<Test, bool>> filter, bool useNoTracking = false)
        {
            throw new NotImplementedException();
        }

        public Task<Test> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Test> UpdateAsync(Test dbRecord)
        {
            throw new NotImplementedException();
        }
    }
}
