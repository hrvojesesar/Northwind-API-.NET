using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Repositories;

public class CustomerCustomerDemoRepository : ICustomerCustomerDemoRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerCustomerDemoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CustomerCustomerDemo>> GetAllCustomerCustomerDemosAsync()
    {
        return await _context.CustomerCustomerDemo.ToListAsync();
    }

    public async Task<CustomerCustomerDemo> GetCustomerCustomerDemoByIdAsync(string? customerId, string? customerTypeId)
    {
        return await _context.CustomerCustomerDemo.FirstOrDefaultAsync(x => x.CustomerID == customerId && x.CustomerTypeID == customerTypeId);
    }

    public async Task<CustomerCustomerDemo> AddCustomerCustomerDemoAsync(CustomerCustomerDemo customerCustomerDemo)
    {
        if (customerCustomerDemo == null)
        {
            throw new ArgumentNullException(nameof(customerCustomerDemo));
        }

        await _context.CustomerCustomerDemo.AddAsync(customerCustomerDemo);
        await _context.SaveChangesAsync();
        return customerCustomerDemo;
    }

    public async Task<bool> DeleteCustomerCustomerDemoAsync(string? customerId, string? customerTypeId)
    {
        var customerCustomerDemo = await _context.CustomerCustomerDemo.FirstOrDefaultAsync(x => x.CustomerID == customerId && x.CustomerTypeID == customerTypeId);

        if (customerCustomerDemo == null)
        {
            return false;
        }

        _context.CustomerCustomerDemo.Remove(customerCustomerDemo);
        await _context.SaveChangesAsync();
        return true;
    }
}