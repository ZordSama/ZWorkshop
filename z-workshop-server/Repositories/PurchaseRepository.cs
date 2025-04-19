using z_workshop_server.Data;
using z_workshop_server.Models;

namespace z_workshop_server.Repositories;

public interface IPurchaseRepository : IRepository<Purchase>;

public interface IPurchaseDetailRepository : IRepository<PurchaseDetail>;

public class PurchaseRepository(AppDbContext db) : Repository<Purchase>(db), IPurchaseRepository { }

public class PurchaseDetailRepository(AppDbContext db)
    : Repository<PurchaseDetail>(db),
        IPurchaseDetailRepository { }
