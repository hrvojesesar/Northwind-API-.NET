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

    public async Task<CustomerCustomerDemo> GetCustomerCustomerDemoByIdAsync(string? customerID, string? customerTypeID)
    {
        return await _context.CustomerCustomerDemo.FirstOrDefaultAsync(x => x.CustomerID == customerID && x.CustomerTypeID == customerTypeID);
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

    public async Task<CustomerCustomerDemo> EditCustomerCustomerDemoAsync(CustomerCustomerDemo customerCustomerDemo)
    {
        var existingCustomerCustomerDemo = await _context.CustomerCustomerDemo.FirstOrDefaultAsync(x => x.CustomerID == customerCustomerDemo.CustomerID && x.CustomerTypeID == customerCustomerDemo.CustomerTypeID);

        if (existingCustomerCustomerDemo == null)
        {
            return null;
        }

        existingCustomerCustomerDemo.CustomerID = customerCustomerDemo.CustomerID;
        existingCustomerCustomerDemo.CustomerTypeID = customerCustomerDemo.CustomerTypeID;

        _context.CustomerCustomerDemo.Update(existingCustomerCustomerDemo);
        await _context.SaveChangesAsync();
        return existingCustomerCustomerDemo;
    }
}
