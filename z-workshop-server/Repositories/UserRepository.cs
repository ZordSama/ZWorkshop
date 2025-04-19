using z_workshop_server.Data;
using z_workshop_server.Models;

namespace z_workshop_server.Repositories;

public interface IUserRepository : IRepository<User>;

public class UserRepository(AppDbContext db) : Repository<User>(db), IUserRepository { }
