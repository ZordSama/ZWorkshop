using z_workshop_server.Data;
using z_workshop_server.Models;

namespace z_workshop_server.Repositories;

public interface IProductRepository : IRepository<Product>;

public class ProductRepository(AppDbContext db) : Repository<Product>(db), IProductRepository { }
