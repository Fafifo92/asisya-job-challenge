using PruebaFullStack.Application.Common.Interfaces;
using PruebaFullStack.Domain.Entities;

namespace PruebaFullStack.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler
{
    private readonly IApplicationDbContext _context;

    public CreateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateCategoryResult> Handle(CreateCategoryCommand command)
    {
        var category = new Category
        {
            CategoryName = command.CategoryName,
            Description = command.Description
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return new CreateCategoryResult { CategoryID = category.CategoryID };
    }
}
