namespace PruebaFullStack.Application.Products.Commands.CreateProduct;

public class CreateProductCommand
{
    public string ProductName { get; set; } = null!;
    public int? CategoryID { get; set; }
    public int? SupplierID { get; set; }
    public decimal? UnitPrice { get; set; }
    public short? UnitsInStock { get; set; }
}

public class CreateProductResult
{
    public int ProductID { get; set; }
}

public class CreateProductsBatchCommand
{
    public int Count { get; set; } = 100000;
}
