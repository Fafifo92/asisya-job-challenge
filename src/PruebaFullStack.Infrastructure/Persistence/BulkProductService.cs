using EFCore.BulkExtensions;
using PruebaFullStack.Application.Common.Interfaces;
using PruebaFullStack.Domain.Entities;

namespace PruebaFullStack.Infrastructure.Persistence;

public class BulkProductService : IBulkProductService
{
    private readonly ApplicationDbContext _context;

    public BulkProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task BulkInsertProductsAsync(List<Product> products)
    {
        await _context.BulkInsertAsync(products);
    }
}
