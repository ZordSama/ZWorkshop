using z_workshop_server.DAL.Data;
using z_workshop_server.DAL.Models;

namespace z_workshop_server.DAL.Repositories;

public interface IProductRepository : IRepository<Product>;

public class ProductRepository(AppDbContext db) : Repository<Product>(db), IProductRepository { }
