using DriverGuide.Domain.Entities;
using DriverGuide.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace DriverGuide.Infrastructure.Database;

public class PytanieSqlRepository : RepositoryBase, IPytanieRepository
{
    public PytanieSqlRepository(AppDbContext context) : base(context) { }

    public Task<Pytanie> CreateAsync(Pytanie dbRecord)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Pytanie dbRecord)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<Pytanie>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Pytanie> GetAsync(Expression<Func<Pytanie, bool>> filter, bool useNoTracking = false)
    {
        throw new NotImplementedException();
    }

    public Task<Pytanie> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Pytanie> UpdateAsync(Pytanie dbRecord)
    {
        throw new NotImplementedException();
    }
}
