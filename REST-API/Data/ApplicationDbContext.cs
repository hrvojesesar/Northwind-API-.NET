using Microsoft.EntityFrameworkCore;
using REST_API.Domain;

namespace REST_API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Shipper> Shippers { get; set; }
    public DbSet<Region> Region { get; set; }
    public DbSet<CustomerDemographic> CustomerDemographics { get; set; }
    public DbSet<Territory> Territories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerCustomerDemo> CustomerCustomerDemo { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeTerritory> EmployeeTerritories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerCustomerDemo>()
           .HasKey(ccd => new { ccd.CustomerID, ccd.CustomerTypeID });

        modelBuilder.Entity<EmployeeTerritory>()
           .HasKey(et => new { et.EmployeeID, et.TerritoryID });

        modelBuilder.Entity<OrderDetail>()
            .HasKey(od => new { od.OrderID, od.ProductID });

        base.OnModelCreating(modelBuilder);
    }

  
}
