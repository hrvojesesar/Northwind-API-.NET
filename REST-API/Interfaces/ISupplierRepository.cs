using REST_API.Domain;

namespace REST_API.Interfaces;

public interface ISupplierRepository
{
    Task<List<Supplier>> GetAllSuppliersAsync();
    Task<Supplier> GetSupplierByIdAsync(int? id);
    Task<Supplier> AddSupplierAsync(Supplier supplier);
    Task<Supplier> EditSupplierAsync(Supplier supplier);
    Task<bool> DeleteSupplierAsync(int? id);
}