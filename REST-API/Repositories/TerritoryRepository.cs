using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Repositories;

public class TerritoryRepository : ITerritoryRepository
{
    private readonly ApplicationDbContext _context;

    public TerritoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Territory>> GetAllTerritoriesAsync()
    {
        var territories = await _context.Territories.ToListAsync();

        foreach (var territory in territories)
        {
            territory.TerritoryDescription = territory.TerritoryDescription.Trim();
        }

        return territories;
    }

    public async Task<Territory> GetTerritoryByIdAsync(string? id)
    {
        var territory = await _context.Territories.FirstOrDefaultAsync(t => t.TerritoryID == id);

        if (territory != null)
        {
            territory.TerritoryDescription = territory.TerritoryDescription.Trim();
        }

        return territory;
    }

    public async Task<Territory> AddTerritoryAsync(Territory territory)
    {
        if (territory == null)
        {
            throw new ArgumentNullException(nameof(territory));
        }

        await _context.Territories.AddAsync(territory);
        await _context.SaveChangesAsync();
        return territory;
    }

    public async Task<Territory> EditTerritoryAsync(Territory territory)
    {
        var existingTerritory = await _context.Territories.FirstOrDefaultAsync(t => t.TerritoryID == territory.TerritoryID);

        if (existingTerritory == null)
        {
            return null;
        }

        existingTerritory.TerritoryDescription = territory.TerritoryDescription;
        existingTerritory.RegionID = territory.RegionID;

        _context.Territories.Update(existingTerritory);
        await _context.SaveChangesAsync();
        return existingTerritory;
    }

    public async Task<bool> DeleteTerritoryAsync(string? id)
    {
        var territory = await _context.Territories.FirstOrDefaultAsync(t => t.TerritoryID == id);

        if (territory == null)
        {
            return false;
        }

        _context.Territories.Remove(territory);
        await _context.SaveChangesAsync();
        return true;
    }
}