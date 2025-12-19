namespace PruebaFullStack.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommand
{
    public string CategoryName { get; set; } = null!;
    public string? Description { get; set; }
}

public class CreateCategoryResult
{
    public int CategoryID { get; set; }
}
