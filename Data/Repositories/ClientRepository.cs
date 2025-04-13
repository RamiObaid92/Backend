using Data.Context;
using Data.Entities;

namespace Data.Repositories;

public interface IClientRepository : IBaseRepository<ClientEntity>
{
}


public class ClientRepository(ApplicationDbContext context) : BaseRepository<ClientEntity>(context), IClientRepository
{
}
