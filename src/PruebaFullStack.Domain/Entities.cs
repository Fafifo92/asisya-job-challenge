namespace PruebaFullStack.Domain.Entities;

public class Product
{
    public int ProductID { get; set; }
    public string ProductName { get; set; } = null!;
    public int? SupplierID { get; set; }
    public int? CategoryID { get; set; }
    public string? QuantityPerUnit { get; set; }
    public decimal? UnitPrice { get; set; }
    public short? UnitsInStock { get; set; }
    public short? UnitsOnOrder { get; set; }
    public short? ReorderLevel { get; set; }
    public bool Discontinued { get; set; }

    public virtual Category? Category { get; set; }
    public virtual Supplier? Supplier { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}

public class Category
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = null!;
    public string? Description { get; set; }
    public byte[]? Picture { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

public class Supplier
{
    public int SupplierID { get; set; }
    public string CompanyName { get; set; } = null!;
    public string? ContactName { get; set; }
    public string? ContactTitle { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? HomePage { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

public class Order
{
    public int OrderID { get; set; }
    public string? CustomerID { get; set; }
    public int? EmployeeID { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? RequiredDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public int? ShipVia { get; set; }
    public decimal? Freight { get; set; }
    public string? ShipName { get; set; }
    public string? ShipAddress { get; set; }
    public string? ShipCity { get; set; }
    public string? ShipRegion { get; set; }
    public string? ShipPostalCode { get; set; }
    public string? ShipCountry { get; set; }

    public virtual Customer? Customer { get; set; }
    public virtual Employee? Employee { get; set; }
    public virtual Shipper? ShipViaNavigation { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}

public class OrderDetail
{
    public int OrderID { get; set; }
    public int ProductID { get; set; }
    public decimal UnitPrice { get; set; }
    public short Quantity { get; set; }
    public float Discount { get; set; }

    public virtual Order Order { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}

public class Employee
{
    public int EmployeeID { get; set; }
    public string LastName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? Title { get; set; }
    public string? TitleOfCourtesy { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? HireDate { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? HomePhone { get; set; }
    public string? Extension { get; set; }
    public byte[]? Photo { get; set; }
    public string? Notes { get; set; }
    public int? ReportsTo { get; set; }
    public string? PhotoPath { get; set; }

    public virtual Employee? ReportsToNavigation { get; set; }
    public virtual ICollection<Employee> InverseReportsToNavigation { get; set; } = new List<Employee>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

public class Customer
{
    public string CustomerID { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public string? ContactName { get; set; }
    public string? ContactTitle { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Region { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}

public class Shipper
{
    public int ShipperID { get; set; }
    public string CompanyName { get; set; } = null!;
    public string? Phone { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
