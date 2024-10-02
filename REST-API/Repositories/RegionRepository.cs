using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Repositories;

public class RegionRepository : IRegionRepository
{
    private readonly ApplicationDbContext _context;

    public RegionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Region>> GetAllRegionsAsync()
    {
        var regions = await _context.Region.ToListAsync();
        regions.ForEach(x => x.RegionDescription = x.RegionDescription.Trim());
        return regions;
    }

    public async Task<Region> GetRegionByIdAsync(int? id)
    {
        var region = await _context.Region.FirstOrDefaultAsync(x => x.RegionID == id);
        if (region != null)
        {
            region.RegionDescription = region.RegionDescription.Trim();
        }
        return region;
    }

    public async Task<Region> AddRegionAsync(Region region)
    {
        if (region == null)
        {
            throw new ArgumentNullException(nameof(region));
        }

        await _context.Region.AddAsync(region);
        await _context.SaveChangesAsync();
        return region;
    }

    public async Task<Region> EditRegionAsync(Region region)
    {
        var regionToUpdate = await _context.Region.FirstOrDefaultAsync(x => x.RegionID == region.RegionID);

        if (regionToUpdate == null)
        {
            return null;
        }

        regionToUpdate.RegionDescription = region.RegionDescription;
        _context.Region.Update(regionToUpdate);
        await _context.SaveChangesAsync();
        return regionToUpdate;
    }

    public async Task<bool> DeleteRegionAsync(int? id)
    {
        var region = await _context.Region.FirstOrDefaultAsync(x => x.RegionID == id);

        if (region == null)
        {
            return false;
        }

        _context.Region.Remove(region);
        await _context.SaveChangesAsync();
        return true;
    }
}