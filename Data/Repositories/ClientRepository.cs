using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories;

public interface IClientRepository : IBaseRepository<ClientEntity>
{
}


public class ClientRepository(ApplicationDbContext context) : BaseRepository<ClientEntity>(context), IClientRepository
{
    private IQueryable<ClientEntity> Includes()
    {
        return _table
            .Include(x => x.Address);
    }

    public override async Task<IEnumerable<ClientEntity>> GetAllAsync()
    {
        return await Includes().ToListAsync();
    }

    public override async Task<ClientEntity?> GetAsync(Expression<Func<ClientEntity, bool>> predicate)
    {
        return await Includes().FirstOrDefaultAsync(predicate);
    }
}
