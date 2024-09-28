using REST_API.Domain;

namespace REST_API.Interfaces;

public interface IShipperRepository
{
    Task<List<Shipper>> GetAllShippersAsync();
    Task<Shipper> GetShipperByIdAsync(int? id);
    Task<Shipper> AddShipperAsync(Shipper shipper);
    Task<Shipper> EditShipperAsync(Shipper shipper);
    Task<bool> DeleteShipperAsync(int? id);
}
