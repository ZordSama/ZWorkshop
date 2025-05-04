using Microsoft.EntityFrameworkCore;
using z_workshop_server.DAL.Data;
using z_workshop_server.DAL.Models;

namespace z_workshop_server.DAL.Repositories;

public interface ILibraryRepository
{
    Task<IEnumerable<Product>> GetProductsAsync(string customerId);
    Task<IEnumerable<Customer>> GetCustomersAsync(string productId);
    Task AddAsync(string customerId, string productId);
    Task RemoveAsync(string customerId, string productId);
}

public class LibraryRepository : ILibraryRepository
{
    private readonly AppDbContext _context;

    public LibraryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync(string customerId)
    {
        return await _context
            .Customers.Where(c => c.CustomerId == customerId)
            .SelectMany(c => c.Products)
            .ToListAsync();
    }

    public async Task<IEnumerable<Customer>> GetCustomersAsync(string productId)
    {
        return await _context
            .Products.Where(p => p.ProductId == productId)
            .SelectMany(p => p.Customers)
            .ToListAsync();
    }

    public async Task AddAsync(string customerId, string productId)
    {
        var exists = await _context
            .Set<Dictionary<string, object>>("Library")
            .FirstOrDefaultAsync(l =>
                l["CustomerId"].Equals(customerId) && l["ProductId"].Equals(productId)
            );

        if (exists == null)
        {
            await _context
                .Set<Dictionary<string, object>>("Library")
                .AddAsync(
                    new Dictionary<string, object>
                    {
                        { "CustomerId", customerId },
                        { "ProductId", productId }
                    }
                );
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveAsync(string customerId, string productId)
    {
        var entry = await _context
            .Set<Dictionary<string, object>>("Library")
            .FirstOrDefaultAsync(l =>
                l["CustomerId"].Equals(customerId) && l["ProductId"].Equals(productId)
            );

        if (entry != null)
        {
            _context.Set<Dictionary<string, object>>("Library").Remove(entry);
            await _context.SaveChangesAsync();
        }
    }
}
