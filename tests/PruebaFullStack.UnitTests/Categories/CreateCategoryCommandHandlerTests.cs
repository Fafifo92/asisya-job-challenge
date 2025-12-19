using Xunit;
using PruebaFullStack.Application.Categories.Commands.CreateCategory;
using PruebaFullStack.Domain.Entities;
using PruebaFullStack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using PruebaFullStack.Application.Common.Interfaces;
using Moq;

namespace PruebaFullStack.UnitTests.Categories;

public class CreateCategoryCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateCategory_WhenValidCommand()
    {
        // Arrange
        // We will mock IApplicationDbContext since we cannot easily use InMemoryDatabase with the interface without a concrete implementation wrapping it.
        // Actually, for unit tests, mocking DbSet is tedious.
        // A better approach for Unit Tests with EF Core is often using the InMemory provider with the real Context,
        // BUT our handler now depends on the Interface.
        // So we can implement a TestDbContext that inherits from ApplicationDbContext and implements the Interface?
        // Or just use the real ApplicationDbContext if it implements the interface.

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "NorthwindTest")
            .Options;

        // ApplicationDbContext implements IApplicationDbContext
        using var context = new ApplicationDbContext(options);

        var handler = new CreateCategoryCommandHandler(context);
        var command = new CreateCategoryCommand
        {
            CategoryName = "Test Category",
            Description = "Test Description"
        };

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.True(result.CategoryID > 0);
        var category = await context.Categories.FindAsync(result.CategoryID);
        Assert.NotNull(category);
        Assert.Equal("Test Category", category.CategoryName);
    }
}
