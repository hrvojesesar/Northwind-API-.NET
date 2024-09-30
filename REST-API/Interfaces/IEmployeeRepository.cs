using REST_API.Domain;

namespace REST_API.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllEmployeesAsync();
    Task<Employee> GetEmployeeByIdAsync(int? id);
    Task<Employee> AddEmployeeAsync(Employee employee);
    Task<Employee> EditEmployeeAsync(Employee employee);
    Task<bool> DeleteEmployeeAsync(int? id);
}
