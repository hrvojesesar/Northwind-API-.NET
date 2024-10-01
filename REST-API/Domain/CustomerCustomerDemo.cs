using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace REST_API.Domain;

public class CustomerCustomerDemo
{
    private readonly ApplicationDbContext _context;

    public CustomerCustomerDemo() { }

    public CustomerCustomerDemo(ApplicationDbContext context)
    {
        _context = context;
    }

    public string CustomerID { get; set; }
    [NotMapped]
    public string? CustomerName { get; set; }
    public string CustomerTypeID { get; set; }
    [NotMapped]
    public string? CustomerTypeDescription { get; set; }

    public async Task LoadCustomerAndCustomerTypeDetailsAsync()
    {
        CustomerName = await _context.Customers
            .Where(x => x.CustomerID == CustomerID)
            .Select(x => x.CompanyName)
            .FirstOrDefaultAsync();

        CustomerTypeDescription = await _context.CustomerDemographics
            .Where(x => x.CustomerTypeID == CustomerTypeID)
            .Select(x => x.CustomerDesc)
            .FirstOrDefaultAsync();
    }
}