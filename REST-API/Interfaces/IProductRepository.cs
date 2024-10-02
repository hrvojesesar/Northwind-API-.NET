using REST_API.Domain;

namespace REST_API.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllProductsAsync();
    Task<Product> GetProductByIdAsync(int? id);
    Task<Product> AddProductAsync(Product product);
    Task<Product> EditProductAsync(Product product);
    Task<bool> DeleteProductAsync(int? id);
}