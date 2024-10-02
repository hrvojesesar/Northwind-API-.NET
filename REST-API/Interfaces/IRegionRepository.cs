using REST_API.Domain;

namespace REST_API.Interfaces;

public interface IRegionRepository
{
    Task<List<Region>> GetAllRegionsAsync();
    Task<Region> GetRegionByIdAsync(int? id);
    Task<Region> AddRegionAsync(Region region);
    Task<Region> EditRegionAsync(Region region);
    Task<bool> DeleteRegionAsync(int? id);
}