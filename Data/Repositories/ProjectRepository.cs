using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories;

public interface IProjectRepository : IBaseRepository<ProjectEntity>
{
}

public class ProjectRepository(ApplicationDbContext context) : BaseRepository<ProjectEntity>(context), IProjectRepository
{
    private IQueryable<ProjectEntity> Includes()
    {
        return _table
            .Include(x => x.Client)
            .Include(x => x.ProjectOwner)
            .Include(x => x.Status);
    }

    public override async Task<IEnumerable<ProjectEntity>> GetAllAsync()
    {
        return await Includes().ToListAsync();
    }

    public override async Task<ProjectEntity?> GetAsync(Expression<Func<ProjectEntity, bool>> predicate)
    {
        return await Includes().FirstOrDefaultAsync();
    }
}
