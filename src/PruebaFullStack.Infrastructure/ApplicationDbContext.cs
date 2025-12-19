using Microsoft.EntityFrameworkCore;
using PruebaFullStack.Application.Common.Interfaces;
using PruebaFullStack.Domain.Entities;

namespace PruebaFullStack.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Supplier> Suppliers { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Shipper> Shippers { get; set; }

    // ... rest of the file (OnModelCreating) remains the same, assuming we don't need to re-type it all if we just update the interface inheritance.
    // However, since I'm overwriting, I must include everything.

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryID);
            entity.Property(e => e.CategoryID).ValueGeneratedOnAdd();
            entity.Property(e => e.CategoryName).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Picture).HasColumnType("bytea");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerID);
            entity.Property(e => e.CustomerID).HasMaxLength(5).IsFixedLength();
            entity.Property(e => e.CompanyName).HasMaxLength(40).IsRequired();
            entity.Property(e => e.ContactName).HasMaxLength(30);
            entity.Property(e => e.ContactTitle).HasMaxLength(30);
            entity.Property(e => e.Address).HasMaxLength(60);
            entity.Property(e => e.City).HasMaxLength(15);
            entity.Property(e => e.Region).HasMaxLength(15);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.Country).HasMaxLength(15);
            entity.Property(e => e.Phone).HasMaxLength(24);
            entity.Property(e => e.Fax).HasMaxLength(24);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeID);
            entity.Property(e => e.EmployeeID).ValueGeneratedOnAdd();
            entity.Property(e => e.LastName).HasMaxLength(20).IsRequired();
            entity.Property(e => e.FirstName).HasMaxLength(10).IsRequired();
            entity.Property(e => e.Title).HasMaxLength(30);
            entity.Property(e => e.TitleOfCourtesy).HasMaxLength(25);
            entity.Property(e => e.BirthDate).HasColumnType("date");
            entity.Property(e => e.HireDate).HasColumnType("date");
            entity.Property(e => e.Address).HasMaxLength(60);
            entity.Property(e => e.City).HasMaxLength(15);
            entity.Property(e => e.Region).HasMaxLength(15);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.Country).HasMaxLength(15);
            entity.Property(e => e.HomePhone).HasMaxLength(24);
            entity.Property(e => e.Extension).HasMaxLength(4);
            entity.Property(e => e.Photo).HasColumnType("bytea");
            entity.Property(e => e.Notes).HasColumnType("text");
            entity.Property(e => e.PhotoPath).HasMaxLength(255);

            entity.HasOne(d => d.ReportsToNavigation)
                .WithMany(p => p.InverseReportsToNavigation)
                .HasForeignKey(d => d.ReportsTo);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderID);
            entity.Property(e => e.OrderID).ValueGeneratedOnAdd();
            entity.Property(e => e.OrderDate).HasColumnType("date");
            entity.Property(e => e.RequiredDate).HasColumnType("date");
            entity.Property(e => e.ShippedDate).HasColumnType("date");
            entity.Property(e => e.Freight).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ShipName).HasMaxLength(40);
            entity.Property(e => e.ShipAddress).HasMaxLength(60);
            entity.Property(e => e.ShipCity).HasMaxLength(15);
            entity.Property(e => e.ShipRegion).HasMaxLength(15);
            entity.Property(e => e.ShipPostalCode).HasMaxLength(10);
            entity.Property(e => e.ShipCountry).HasMaxLength(15);
            entity.Property(e => e.CustomerID).HasMaxLength(5).IsFixedLength();

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerID);

            entity.HasOne(d => d.Employee)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.EmployeeID);

            entity.HasOne(d => d.ShipViaNavigation)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.ShipVia);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderID, e.ProductID });
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)").IsRequired();
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.Discount).IsRequired();

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderID);

            entity.HasOne(d => d.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductID);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductID);
            entity.Property(e => e.ProductID).ValueGeneratedOnAdd();
            entity.Property(e => e.ProductName).HasMaxLength(40).IsRequired();
            entity.Property(e => e.QuantityPerUnit).HasMaxLength(20);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UnitsInStock).HasDefaultValue((short)0);
            entity.Property(e => e.UnitsOnOrder).HasDefaultValue((short)0);
            entity.Property(e => e.ReorderLevel).HasDefaultValue((short)0);
            entity.Property(e => e.Discontinued).IsRequired();

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryID);

            entity.HasOne(d => d.Supplier)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierID);
        });

        modelBuilder.Entity<Shipper>(entity =>
        {
            entity.HasKey(e => e.ShipperID);
            entity.Property(e => e.ShipperID).ValueGeneratedOnAdd();
            entity.Property(e => e.CompanyName).HasMaxLength(40).IsRequired();
            entity.Property(e => e.Phone).HasMaxLength(24);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierID);
            entity.Property(e => e.SupplierID).ValueGeneratedOnAdd();
            entity.Property(e => e.CompanyName).HasMaxLength(40).IsRequired();
            entity.Property(e => e.ContactName).HasMaxLength(30);
            entity.Property(e => e.ContactTitle).HasMaxLength(30);
            entity.Property(e => e.Address).HasMaxLength(60);
            entity.Property(e => e.City).HasMaxLength(15);
            entity.Property(e => e.Region).HasMaxLength(15);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.Country).HasMaxLength(15);
            entity.Property(e => e.Phone).HasMaxLength(24);
            entity.Property(e => e.Fax).HasMaxLength(24);
            entity.Property(e => e.HomePage).HasColumnType("text");
        });
    }
}
