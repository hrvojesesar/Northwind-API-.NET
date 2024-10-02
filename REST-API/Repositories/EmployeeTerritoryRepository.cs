using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Repositories;

public class EmployeeTerritoryRepository : IEmployeeTerritoryRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeTerritoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<EmployeeTerritory>> GetAllEmployeeTerritoriesAsync()
    {
        return await _context.EmployeeTerritories.ToListAsync();
    }

    public async Task<EmployeeTerritory> GetEmployeeTerritoryByIdAsync(int? employeeId, string? territoryId)
    {
        return await _context.EmployeeTerritories.FirstOrDefaultAsync(x => x.EmployeeID == employeeId && x.TerritoryID == territoryId);
    }

    public async Task<EmployeeTerritory> AddEmployeeTerritoryAsync(EmployeeTerritory employeeTerritory)
    {
        if (employeeTerritory == null)
        {
            throw new ArgumentNullException(nameof(employeeTerritory));
        }

        await _context.EmployeeTerritories.AddAsync(employeeTerritory);
        await _context.SaveChangesAsync();
        return employeeTerritory;
    }

    public async Task<bool> DeleteEmployeeTerritoryAsync(int? employeeId, string? territoryId)
    {
        var employeeTerritory = await _context.EmployeeTerritories.FirstOrDefaultAsync(x => x.EmployeeID == employeeId && x.TerritoryID == territoryId);
        if (employeeTerritory == null)
        {
            return false;
        }

        _context.EmployeeTerritories.Remove(employeeTerritory);
        await _context.SaveChangesAsync();
        return true;
    }
}