using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaFullStack.Application.Categories.Commands.CreateCategory;
using PruebaFullStack.Infrastructure.Persistence;

namespace PruebaFullStack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
    {
        var handler = new CreateCategoryCommandHandler(_context);
        var result = await handler.Handle(command);
        return Ok(result);
    }
}
