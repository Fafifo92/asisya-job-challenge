using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaFullStack.Application.Common.Interfaces;
using PruebaFullStack.Application.Products.Commands.CreateProduct;
using PruebaFullStack.Application.Products.Queries.GetProducts;

namespace PruebaFullStack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IApplicationDbContext _context;
    private readonly IBulkProductService _bulkService;

    public ProductController(IApplicationDbContext context, IBulkProductService bulkService)
    {
        _context = context;
        _bulkService = bulkService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetProductsQuery query)
    {
        var handler = new GetProductsQueryHandler(_context);
        var result = await handler.Handle(query);
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var handler = new CreateProductCommandHandler(_context, _bulkService);
        var result = await handler.Handle(command);
        return Ok(result);
    }

    [HttpPost("seed")]
    [Authorize]
    public async Task<IActionResult> CreateBatch([FromBody] CreateProductsBatchCommand command)
    {
        var handler = new CreateProductCommandHandler(_context, _bulkService);
        await handler.HandleBatch(command);
        return Ok(new { Message = $"Successfully seeded {command.Count} products." });
    }
}
