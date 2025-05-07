using DriverGuide.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DriverGuide.Infrastructure.Database;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly DriverGuideDbContext Context;
    protected readonly DbSet<T> DBSet;

    public RepositoryBase(DriverGuideDbContext context)
    {
        Context = context;
        DBSet = Context.Set<T>();
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await DBSet.FindAsync(id);
    }

    public virtual async Task<T> GetByGuidAsync(Guid guid)
    {
        return await DBSet.FindAsync(guid);
    }

    public virtual async Task<ICollection<T>> GetAllAsync()
    {
        return await DBSet.ToListAsync();
    }

    public virtual async Task<T> GetAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false)
    {
        if (useNoTracking)
        {
            return await DBSet.AsNoTracking().FirstOrDefaultAsync(filter);
        }
        else
        {
            return await DBSet.FirstOrDefaultAsync(filter);
        }
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        await DBSet.AddAsync(entity);
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
        DBSet.Remove(entity);
        await Context.SaveChangesAsync();
        return true;
    }
}
