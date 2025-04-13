using Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories;

public abstract class BaseRepository<TEntity> where TEntity : class
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

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var entities = await _table.ToListAsync();
        return entities;
    }

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entity = await _table.FirstOrDefaultAsync(predicate);
        return entity;
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        if (entity == null) return false;

        _table.Update(entity);
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

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var result = await _table.AnyAsync(predicate);
        return result;
    }
}


