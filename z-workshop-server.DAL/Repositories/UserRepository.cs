using z_workshop_server.DAL.Data;
using z_workshop_server.DAL.Models;

namespace z_workshop_server.DAL.Repositories;

public interface IUserRepository : IRepository<User>;

public class UserRepository(AppDbContext db) : Repository<User>(db), IUserRepository { }
