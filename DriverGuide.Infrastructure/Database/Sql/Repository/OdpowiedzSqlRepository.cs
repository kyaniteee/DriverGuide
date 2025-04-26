using DriverGuide.Domain.Entities;
using DriverGuide.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace DriverGuide.Infrastructure.Database;

public class OdpowiedzSqlRepository : RepositoryBase, IOdpowiedzRepository
{
    public OdpowiedzSqlRepository(AppDbContext context) : base(context) { }

    public Task<Odpowiedz> CreateAsync(Odpowiedz dbRecord)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Odpowiedz dbRecord)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<Odpowiedz>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Odpowiedz> GetAsync(Expression<Func<Odpowiedz, bool>> filter, bool useNoTracking = false)
    {
        throw new NotImplementedException();
    }

    public Task<Odpowiedz> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Odpowiedz> UpdateAsync(Odpowiedz dbRecord)
    {
        throw new NotImplementedException();
    }
}
