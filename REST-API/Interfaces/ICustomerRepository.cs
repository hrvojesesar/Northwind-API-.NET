using REST_API.Domain;

namespace REST_API.Interfaces;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllCustomersAsync();
    Task<Customer> GetCustomerByIdAsync(string? customerId);
    Task<Customer> AddCustomerAsync(Customer customer);
    Task<Customer> EditCustomerAsync(Customer customer);
    Task<bool> DeleteCustomerAsync(string? customerId);

}