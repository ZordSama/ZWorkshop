using z_workshop_server.DAL.Data;
using z_workshop_server.DAL.Models;

namespace z_workshop_server.DAL.Repositories;

public interface IEmployeeRepository : IRepository<Employee> { }

public class EmployeeRepository(AppDbContext db) : Repository<Employee>(db), IEmployeeRepository { }
