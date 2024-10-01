using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace REST_API.Domain;

public class EmployeeTerritory
{
    private readonly ApplicationDbContext _context;

    public EmployeeTerritory() { }

    public EmployeeTerritory(ApplicationDbContext context)
    {
        _context = context;
    }

    public int EmployeeID { get; set; }
    [NotMapped]
    public string? Employee { get; set; }
    public string TerritoryID { get; set; }
    [NotMapped]
    public string? Territory { get; set; }

    public async Task LoadEmployeeAndTerritoryDetailsAsync()
    {
        Employee = await _context.Employees
            .Where(x => x.EmployeeID == EmployeeID)
            .Select(x => x.FirstName + " " + x.LastName)
            .FirstOrDefaultAsync();

        Territory = await _context.Territories
            .Where(x => x.TerritoryID == TerritoryID)
            .Select(x => x.TerritoryDescription)
            .FirstOrDefaultAsync();
    }
}
