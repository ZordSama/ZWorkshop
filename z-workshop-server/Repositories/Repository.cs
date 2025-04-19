using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using z_workshop_server.Data;

namespace z_workshop_server.Repositories;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<TEntity?> GetByIdAsync(params object[] keys);
    Task<TEntity?> GetByProperty<TProperty>(
        Expression<Func<TEntity, TProperty>> propertySelector,
        TProperty value
    );
    Task<List<TEntity>> GetAllByProperty<TProperty>(
        Expression<Func<TEntity, TProperty>> propertySelector,
        TProperty value
    );
    Task<List<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}

public class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    protected readonly AppDbContext _dbContext;
    protected readonly DbSet<TEntity> _dbSet;

    public Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(params object[] keys)
    {
        return await _dbSet.FindAsync(keys);
    }

    public async Task<TEntity?> GetByProperty<TProperty>(
        Expression<Func<TEntity, TProperty>> propertySelector,
        TProperty value
    )
    {
        if (propertySelector.Body is not MemberExpression memberExpr)
            throw new ArgumentException("The property selector must be a member expression.");

        var propertyName = memberExpr.Member.Name;

        var entity = await _dbSet.FirstOrDefaultAsync(e =>
            EF.Property<TProperty>(e, propertyName)!.Equals(value)
        );

        return entity;
    }

    public async Task<List<TEntity>> GetAllByProperty<TProperty>(
        Expression<Func<TEntity, TProperty>> propertySelector,
        TProperty value
    )
    {
        if (propertySelector.Body is not MemberExpression memberExpr)
            throw new ArgumentException("The property selector must be a member expression.");

        var propertyName = memberExpr.Member.Name;

        return await _dbSet
            .Where(e => EF.Property<TProperty>(e, propertyName)!.Equals(value))
            .ToListAsync();
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
}
