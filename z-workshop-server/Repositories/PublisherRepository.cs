using z_workshop_server.Data;
using z_workshop_server.Models;

namespace z_workshop_server.Repositories;

public interface IPublisherRepository : IRepository<Publisher, string>;

public class PublisherRepository(AppDbContext db)
    : Repository<Publisher, string>(db),
        IPublisherRepository { }
