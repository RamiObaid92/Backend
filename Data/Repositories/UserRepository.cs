using Data.Context;
using Data.Entities;

namespace Data.Repositories;

public interface IUserRepository : IBaseRepository<UserEntity>
{
}

public class UserRepository(ApplicationDbContext context) : BaseRepository<UserEntity>(context), IUserRepository
{
}