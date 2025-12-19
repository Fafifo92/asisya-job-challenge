using PruebaFullStack.Application.Common.Interfaces;
using PruebaFullStack.Domain.Entities;

namespace PruebaFullStack.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IBulkProductService _bulkService;

    public CreateProductCommandHandler(IApplicationDbContext context, IBulkProductService bulkService)
    {
        _context = context;
        _bulkService = bulkService;
    }

    public async Task<CreateProductResult> Handle(CreateProductCommand command)
    {
        var product = new Product
        {
            ProductName = command.ProductName,
            CategoryID = command.CategoryID,
            SupplierID = command.SupplierID,
            UnitPrice = command.UnitPrice,
            UnitsInStock = command.UnitsInStock,
            Discontinued = false
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return new CreateProductResult { ProductID = product.ProductID };
    }

    public async Task HandleBatch(CreateProductsBatchCommand command)
    {
        var products = new List<Product>(command.Count);
        var random = new Random();

        for (int i = 0; i < command.Count; i++)
        {
            products.Add(new Product
            {
                ProductName = $"Product {Guid.NewGuid().ToString().Substring(0, 8)}",
                CategoryID = random.Next(1, 8), // Assuming Categories 1-8 exist
                UnitPrice = (decimal)(random.NextDouble() * 100),
                UnitsInStock = (short)random.Next(0, 100),
                Discontinued = false
            });
        }

        await _bulkService.BulkInsertProductsAsync(products);
    }
}
