using REST_API.Domain;

namespace REST_API.Interfaces;

public interface ITerritoryRepository
{
    Task<List<Territory>> GetAllTerritoriesAsync();
    Task<Territory> GetTerritoryByIdAsync(string? id);
    Task<Territory> AddTerritoryAsync(Territory territory);
    Task<Territory> EditTerritoryAsync(Territory territory);
    Task<bool> DeleteTerritoryAsync(string? id);

}