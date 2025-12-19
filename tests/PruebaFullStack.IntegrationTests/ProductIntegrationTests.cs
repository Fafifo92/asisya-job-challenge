using System.Data.Common;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using PruebaFullStack.Application.Categories.Commands.CreateCategory;
using PruebaFullStack.Application.Products.Queries.GetProducts;
using PruebaFullStack.Infrastructure.Persistence;
using Testcontainers.PostgreSql;
using Xunit;

namespace PruebaFullStack.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithDatabase("northwind_test")
        .WithUsername("test_user")
        .WithPassword("test_password")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext options
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.RemoveAll(typeof(DbConnection)); // Remove any existing connection registration

            // Register DbContext with Testcontainers connection string
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString()));
        });

        // Suppress logging during tests to keep output clean, but allow critical errors
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.SetMinimumLevel(LogLevel.Error);
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        // Create a scope to resolve the DbContext and apply migrations
        // Using 'Services' property ensures the application is bootstrapped and our overrides are applied.
        using var scope = Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            // Log connection string for debugging (be careful with secrets in real apps, ok for test env)
            // var logger = services.GetRequiredService<ILogger<CustomWebApplicationFactory>>();
            // logger.LogError($"DEBUG: Connection String: {_dbContainer.GetConnectionString()}");

            // Force migration
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            // Fallback or explicit failure
            Console.WriteLine($"Migration failed: {ex.Message}");
            throw;
        }
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}

public class ProductIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ProductIntegrationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateCategoryAndListProducts_ShouldWorkCorrectly()
    {
        // 1. Login to get token
        var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", new { Username = "admin", Password = "password" });
        loginResponse.EnsureSuccessStatusCode();
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResult>();
        var token = loginResult!.Token;

        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // 2. Create Category
        var createCategoryCommand = new CreateCategoryCommand { CategoryName = "Integration Test Category", Description = "Desc" };
        var categoryResponse = await _client.PostAsJsonAsync("/api/Category", createCategoryCommand);

        if (!categoryResponse.IsSuccessStatusCode)
        {
            var errorContent = await categoryResponse.Content.ReadAsStringAsync();
            throw new Exception($"Create Category Failed: {categoryResponse.StatusCode} - {errorContent}");
        }

        var categoryResult = await categoryResponse.Content.ReadFromJsonAsync<CreateCategoryResult>();
        Assert.True(categoryResult!.CategoryID > 0);

        // 3. Create Product
        var createProductCommand = new { ProductName = "Integration Prod", CategoryID = categoryResult.CategoryID, UnitPrice = 10.5m, UnitsInStock = 100 };
        var productResponse = await _client.PostAsJsonAsync("/api/Product", createProductCommand);
        productResponse.EnsureSuccessStatusCode();

        // 4. Get Products
        var getResponse = await _client.GetAsync("/api/Product?PageNumber=1&PageSize=10");
        getResponse.EnsureSuccessStatusCode();
        var getResult = await getResponse.Content.ReadFromJsonAsync<PaginatedResult<ProductDto>>();

        Assert.NotNull(getResult);
        Assert.Contains(getResult.Items, p => p.ProductName == "Integration Prod" && p.CategoryName == "Integration Test Category");
    }

    record LoginResult(string Token);
}
