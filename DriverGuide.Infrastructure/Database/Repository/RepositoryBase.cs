using DriverGuide.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DriverGuide.Infrastructure.Database;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly DriverGuideDbContext Context;
    private readonly DbSet<T> _dbSet;

    public RepositoryBase(DriverGuideDbContext context)
    {
        Context = context;
        _dbSet = Context.Set<T>();
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<T> GetByGuidAsync(Guid guid)
    {
        return await _dbSet.FindAsync(guid);
    }

    public virtual async Task<ICollection<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T> GetAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false)
    {
        if (useNoTracking)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(filter);
        }
        else
        {
            return await _dbSet.FirstOrDefaultAsync(filter);
        }
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        Context.Entry(entity).State = EntityState.Modified;
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task<bool> DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await Context.SaveChangesAsync();
        return true;
    }

}
