using z_workshop_server.Data;
using z_workshop_server.Models;

namespace z_workshop_server.Repositories;

public interface ICustomerRepository : IRepository<Customer, string>;

public class CustomerRepository(AppDbContext db)
    : Repository<Customer, string>(db),
        ICustomerRepository { }
