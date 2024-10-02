using REST_API.Domain;

namespace REST_API.Interfaces;

public interface ICustomerDemographicRepository
{
    Task<List<CustomerDemographic>> GetAllCustomerDemographicsAsync();
    Task<CustomerDemographic> GetCustomerDemographicByIdAsync(string? id);
    Task<CustomerDemographic> AddCustomerDemographicAsync(CustomerDemographic customerDemographic);
    Task<CustomerDemographic> EditCustomerDemographicAsync(CustomerDemographic customerDemographic);
    Task<bool> DeleteCustomerDemographicAsync(string? id);
}