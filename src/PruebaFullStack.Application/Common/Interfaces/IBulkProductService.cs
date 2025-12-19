using PruebaFullStack.Domain.Entities;

namespace PruebaFullStack.Application.Common.Interfaces;

public interface IBulkProductService
{
    Task BulkInsertProductsAsync(List<Product> products);
}
