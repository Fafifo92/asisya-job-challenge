using Microsoft.EntityFrameworkCore;
using PruebaFullStack.Domain.Entities;

namespace PruebaFullStack.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
    DbSet<Category> Categories { get; }
    DbSet<Supplier> Suppliers { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderDetail> OrderDetails { get; }
    DbSet<Employee> Employees { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Shipper> Shippers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
