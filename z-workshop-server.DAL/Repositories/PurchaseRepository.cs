using z_workshop_server.DAL.Data;
using z_workshop_server.DAL.Models;

namespace z_workshop_server.DAL.Repositories;

public interface IPurchaseRepository : IRepository<Purchase>;

public interface IPurchaseDetailRepository : IRepository<PurchaseDetail>;

public class PurchaseRepository(AppDbContext db) : Repository<Purchase>(db), IPurchaseRepository { }

public class PurchaseDetailRepository(AppDbContext db)
    : Repository<PurchaseDetail>(db),
        IPurchaseDetailRepository { }
