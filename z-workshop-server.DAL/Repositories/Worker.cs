using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using z_workshop_server.DAL.Data;

namespace z_workshop_server.DAL.Repositories;

public interface IWorker : IDisposable
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task SaveChangesAsync();
}

public class Worker : IWorker
{
    private readonly AppDbContext _db;

    public Worker(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _db.Database.BeginTransactionAsync();
    }

    public async Task SaveChangesAsync()
    {
        ApplyTimestamps();
        await _db.SaveChangesAsync();
    }

    public void Dispose()
    {
        _db.Dispose();
    }

    private void ApplyTimestamps()
    {
        var entries = _db
            .ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            var entity = entry.Entity;
            var entityType = entity.GetType();

            var createdAtProp = entityType.GetProperty("CreatedAt");
            var lastUpdateProp = entityType.GetProperty("LastUpdate");

            if (entry.State == EntityState.Added && createdAtProp != null && createdAtProp.CanWrite)
            {
                createdAtProp.SetValue(entity, DateTime.UtcNow);
            }

            if (lastUpdateProp != null && lastUpdateProp.CanWrite)
            {
                lastUpdateProp.SetValue(entity, DateTime.UtcNow);
            }
        }
    }
}
