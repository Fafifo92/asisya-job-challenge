namespace PruebaFullStack.Application.Products.Queries.GetProducts;

public class GetProductsQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
    public int? CategoryID { get; set; }
}

public class ProductDto
{
    public int ProductID { get; set; }
    public string ProductName { get; set; } = null!;
    public string? CategoryName { get; set; }
    public decimal? UnitPrice { get; set; }
    public short? UnitsInStock { get; set; }
}

public class PaginatedResult<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
