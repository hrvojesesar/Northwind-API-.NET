using REST_API.Domain;

namespace REST_API.Interfaces;

public interface ICustomerCustomerDemoRepository
{
    Task<List<CustomerCustomerDemo>> GetAllCustomerCustomerDemosAsync();
    Task<CustomerCustomerDemo> GetCustomerCustomerDemoByIdAsync(string? customerId, string? customerTypeId);
    Task<CustomerCustomerDemo> AddCustomerCustomerDemoAsync(CustomerCustomerDemo customerCustomerDemo);
    Task<bool> DeleteCustomerCustomerDemoAsync(string? customerId, string? customerTypeId);
}