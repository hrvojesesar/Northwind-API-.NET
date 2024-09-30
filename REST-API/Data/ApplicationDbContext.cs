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
}
