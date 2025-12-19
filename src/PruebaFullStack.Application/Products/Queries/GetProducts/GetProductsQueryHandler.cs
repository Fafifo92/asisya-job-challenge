using Microsoft.EntityFrameworkCore;
using PruebaFullStack.Application.Common.Interfaces;

namespace PruebaFullStack.Application.Products.Queries.GetProducts;

public class GetProductsQueryHandler
{
    private readonly IApplicationDbContext _context;

    public GetProductsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<ProductDto>> Handle(GetProductsQuery query)
    {
        var queryable = _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(query.SearchTerm))
        {
            queryable = queryable.Where(p => p.ProductName.Contains(query.SearchTerm));
        }

        if (query.CategoryID.HasValue)
        {
            queryable = queryable.Where(p => p.CategoryID == query.CategoryID);
        }

        var totalCount = await queryable.CountAsync();

        var items = await queryable
            .OrderBy(p => p.ProductName)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(p => new ProductDto
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                CategoryName = p.Category != null ? p.Category.CategoryName : null,
                UnitPrice = p.UnitPrice,
                UnitsInStock = p.UnitsInStock
            })
            .ToListAsync();

        return new PaginatedResult<ProductDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }
}
