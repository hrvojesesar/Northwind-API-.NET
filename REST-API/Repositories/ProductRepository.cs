using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(int? id)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.ProductID == id);
    }

    public async Task<Product> AddProductAsync(Product product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> EditProductAsync(Product product)
    {
        var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == product.ProductID);
        if (existingProduct == null)
        {
            return null;
        }

        existingProduct.ProductName = product.ProductName;
        existingProduct.SupplierID = product.SupplierID;
        existingProduct.CategoryID = product.CategoryID;
        existingProduct.QuantityPerUnit = product.QuantityPerUnit;
        existingProduct.UnitPrice = product.UnitPrice;
        existingProduct.UnitsInStock = product.UnitsInStock;
        existingProduct.UnitsOnOrder = product.UnitsOnOrder;
        existingProduct.ReorderLevel = product.ReorderLevel;
        existingProduct.Discontinued = product.Discontinued;

        _context.Products.Update(existingProduct);
        await _context.SaveChangesAsync();
        return existingProduct;
    }

    public async Task<bool> DeleteProductAsync(int? id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == id);
        if (product == null)
        {
            return false;
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

}
