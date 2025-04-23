using z_workshop_server.DAL.Data;
using z_workshop_server.DAL.Models;

namespace z_workshop_server.DAL.Repositories;

public interface ICommentRepository : IRepository<Comment>;

public class CommentRepository(AppDbContext db) : Repository<Comment>(db), ICommentRepository { }
