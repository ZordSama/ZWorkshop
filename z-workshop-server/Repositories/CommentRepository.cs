using z_workshop_server.Data;
using z_workshop_server.Models;

namespace z_workshop_server.Repositories;

public interface ICommentRepository : IRepository<Comment, string>;

public class CommentRepository(AppDbContext db)
    : Repository<Comment, string>(db),
        ICommentRepository { }
