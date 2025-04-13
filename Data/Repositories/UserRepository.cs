using Data.Context;
using Data.Entities;
using System.Runtime.CompilerServices;

namespace Data.Repositories;

public interface IUserRepository : IBaseRepository<UserEntity>
{
}

public class UserRepository(ApplicationDbContext context) : BaseRepository<UserEntity>(context), IUserRepository
{
}