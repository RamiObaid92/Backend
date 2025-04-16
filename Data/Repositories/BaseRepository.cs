using Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<bool> AddAsync(TEntity entity);
    Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> UpdateAsync(TEntity entity);
}

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly DbSet<TEntity> _table;
    protected readonly ApplicationDbContext _context;

    protected BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        _table = _context.Set<TEntity>();
    }

    public virtual async Task<bool> AddAsync(TEntity entity)
    {
        if (entity == null) return false;

        await _table.AddAsync(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        if (predicate == null) return false;

        var entity = await _table.FirstOrDefaultAsync(predicate);
        if (entity == null) return false;

        _table.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate) => await _table.AnyAsync(predicate);

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => await _table.ToListAsync();

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate) => await _table.FirstOrDefaultAsync(predicate);

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        if (entity == null) return false;

        _table.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
