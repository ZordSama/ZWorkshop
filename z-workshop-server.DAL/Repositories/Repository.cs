using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using z_workshop_server.DAL.Data;

namespace z_workshop_server.DAL.Repositories;

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
    Task<TEntity?> GetByIdWithIncludesAsync(
        object[] keys,
        params Expression<Func<TEntity, object>>[] includes
    );
    Task<TEntity?> GetByPropertyWithIncludesAsync<TProperty>(
        Expression<Func<TEntity, TProperty>> propertySelector,
        TProperty value,
        params Expression<Func<TEntity, object>>[] includes
    );
    Task<List<TEntity>> GetAllWithIncludesAsync(
        params Expression<Func<TEntity, object>>[] includes
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

    public async Task<TEntity?> GetByIdWithIncludesAsync(
        object[] keys,
        params Expression<Func<TEntity, object>>[] includes
    )
    {
        IQueryable<TEntity> query = _dbSet;
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        // FindAsync doesn't work directly with IQueryable includes easily,
        // so fetch based on key properties. This assumes a single key for simplicity.
        // You might need a more robust way to handle composite keys if applicable.
        var keyProperty = _dbContext
            .Model.FindEntityType(typeof(TEntity))
            ?.FindPrimaryKey()
            ?.Properties.FirstOrDefault();
        if (keyProperty == null || keys.Length != 1)
        {
            throw new NotSupportedException(
                "GetByIdWithIncludesAsync currently supports single primary keys only or entity type not found."
            );
        }
        var parameter = Expression.Parameter(typeof(TEntity), "e");
        var propertyAccess = Expression.Property(parameter, keyProperty.Name);
        var keyEquality = Expression.Equal(propertyAccess, Expression.Constant(keys[0]));
        var lambda = Expression.Lambda<Func<TEntity, bool>>(keyEquality, parameter);

        return await query.FirstOrDefaultAsync(lambda);
    }

    public async Task<TEntity?> GetByPropertyWithIncludesAsync<TProperty>(
        Expression<Func<TEntity, TProperty>> propertySelector,
        TProperty value,
        params Expression<Func<TEntity, object>>[] includes
    )
    {
        IQueryable<TEntity> query = _dbSet;
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        // Combine the original property selector with the includes
        return await query.FirstOrDefaultAsync(e =>
            EF.Property<TProperty>(e, ((MemberExpression)propertySelector.Body).Member.Name)!
                .Equals(value)
        );
    }

    public async Task<List<TEntity>> GetAllWithIncludesAsync(
        params Expression<Func<TEntity, object>>[] includes
    )
    {
        IQueryable<TEntity> query = _dbSet;
        foreach (var include in includes)
        {
            query = query.Include(include);
        }
        return await query.ToListAsync();
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
