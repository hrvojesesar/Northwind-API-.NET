using REST_API.Domain;

namespace REST_API.Interfaces;

public interface IEmployeeTerritoryRepository
{
    Task<List<EmployeeTerritory>> GetAllEmployeeTerritoriesAsync();
    Task<EmployeeTerritory> GetEmployeeTerritoryByIdAsync(int? employeeId, string? territoryId);
    Task<EmployeeTerritory> AddEmployeeTerritoryAsync(EmployeeTerritory employeeTerritory);
    Task<bool> DeleteEmployeeTerritoryAsync(int? employeeId, string? territoryId);
}
