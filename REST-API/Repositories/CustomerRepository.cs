using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllCustomersAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer> GetCustomerByIdAsync(string? customerId)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerID == customerId);
    }

    public async Task<Customer> AddCustomerAsync(Customer customer)
    {
        if (customer == null)
        {
            throw new ArgumentNullException(nameof(customer));
        }

        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer> EditCustomerAsync(Customer customer)
    {
        var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerID == customer.CustomerID);

        if (existingCustomer == null)
        {
            return null;
        }

        existingCustomer.CompanyName = customer.CompanyName;
        existingCustomer.ContactName = customer.ContactName;
        existingCustomer.ContactTitle = customer.ContactTitle;
        existingCustomer.Address = customer.Address;
        existingCustomer.City = customer.City;
        existingCustomer.Region = customer.Region;
        existingCustomer.PostalCode = customer.PostalCode;
        existingCustomer.Country = customer.Country;
        existingCustomer.Phone = customer.Phone;
        existingCustomer.Fax = customer.Fax;

        _context.Customers.Update(existingCustomer);
        await _context.SaveChangesAsync();
        return existingCustomer;

    }

    public async Task<bool> DeleteCustomerAsync(string? customerId)
    {
        var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerID == customerId);

        if (existingCustomer == null)
        {
            return false;
        }

        _context.Customers.Remove(existingCustomer);
        await _context.SaveChangesAsync();
        return true;
    }
}

