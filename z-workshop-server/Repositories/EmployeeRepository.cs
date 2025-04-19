using z_workshop_server.Data;
using z_workshop_server.Models;

namespace z_workshop_server.Repositories;

public interface IEmployeeRepository : IRepository<Employee> { }

public class EmployeeRepository(AppDbContext db) : Repository<Employee>(db), IEmployeeRepository { }
