using z_workshop_server.DAL.Data;
using z_workshop_server.DAL.Models;

namespace z_workshop_server.DAL.Repositories;

public interface ICustomerRepository : IRepository<Customer>;

public class CustomerRepository(AppDbContext db) : Repository<Customer>(db), ICustomerRepository { }
