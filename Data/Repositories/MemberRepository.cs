using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data.Repositories;

public interface IMemberRepository : IBaseRepository<MemberEntity>
{
}

public class MemberRepository(ApplicationDbContext context) : BaseRepository<MemberEntity>(context), IMemberRepository
{
    private IQueryable<MemberEntity> Includes()
    {
        return _table
            .Include(x => x.Address);
    }

    public override async Task<IEnumerable<MemberEntity>> GetAllAsync()
    {
        return await Includes().ToListAsync();
    }

    public override async Task<MemberEntity?> GetAsync(Expression<Func<MemberEntity, bool>> predicate)
    {
        return await Includes().FirstOrDefaultAsync(predicate);
    }
}