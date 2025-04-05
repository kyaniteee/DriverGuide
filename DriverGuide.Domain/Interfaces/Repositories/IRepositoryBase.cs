using System.Linq.Expressions;

namespace DriverGuide.Domain.Interfaces.Repositories
{
    public interface IRepositoryBase<T>
    {
        Task<T> GetByIdAsync(Guid id);
        Task<ICollection<T>> GetAllAsync();
        Task<T> GetAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);
        Task<T> CreateAsync(T dbRecord);
        Task<T> UpdateAsync(T dbRecord);
        Task<bool> DeleteAsync(T dbRecord);
    }
}
