using DriverGuide.Domain.Interfaces;
using DriverGuide.Domain.Models;
using System.Linq.Expressions;

namespace DriverGuide.Infrastructure.Database;

public class UserSqlRepository : RepositoryBase, IUserRepository
{
    public UserSqlRepository(DriverGuideDbContext context) : base(context) { }

    public Task<User> CreateAsync(User dbRecord)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(User dbRecord)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<User> GetAsync(Expression<Func<User, bool>> filter, bool useNoTracking = false)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateAsync(User dbRecord)
    {
        throw new NotImplementedException();
    }
}
