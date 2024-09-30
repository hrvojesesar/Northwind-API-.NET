using REST_API.Domain;

namespace REST_API.Interfaces;

public interface ICustomerCustomerDemoRepository
{
    Task<List<CustomerCustomerDemo>> GetAllCustomerCustomerDemosAsync();
    Task<CustomerCustomerDemo> GetCustomerCustomerDemoByIdAsync(string? customerID, string? customerTypeID);
    Task<CustomerCustomerDemo> AddCustomerCustomerDemoAsync(CustomerCustomerDemo customerCustomerDemo);
    Task<bool> DeleteCustomerCustomerDemoAsync(string? customerID, string? customerTypeID);
}
