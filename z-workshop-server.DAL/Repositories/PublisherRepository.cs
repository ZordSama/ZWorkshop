using z_workshop_server.DAL.Data;
using z_workshop_server.DAL.Models;

namespace z_workshop_server.DAL.Repositories;

public interface IPublisherRepository : IRepository<Publisher>;

public class PublisherRepository(AppDbContext db)
    : Repository<Publisher>(db),
        IPublisherRepository { }
