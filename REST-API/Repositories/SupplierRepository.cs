using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Repositories;

public class SupplierRepository : ISupplierRepository
{
    private readonly ApplicationDbContext _context;

    public SupplierRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Supplier>> GetAllSuppliersAsync()
    {
        return await _context.Suppliers.ToListAsync();
    }

    public async Task<Supplier> GetSupplierByIdAsync(int? id)
    {
        return await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierID == id);
    }

    public async Task<Supplier> AddSupplierAsync(Supplier supplier)
    {
        if (supplier == null)
        {
            throw new ArgumentNullException(nameof(supplier));
        }

        await _context.Suppliers.AddAsync(supplier);
        await _context.SaveChangesAsync();
        return supplier;
    }

    public async Task<Supplier> EditSupplierAsync(Supplier supplier)
    {
        var existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierID == supplier.SupplierID);

        if (existingSupplier == null)
        {
            return null;
        }

        existingSupplier.CompanyName = supplier.CompanyName;
        existingSupplier.ContactName = supplier.ContactName;
        existingSupplier.ContactTitle = supplier.ContactTitle;
        existingSupplier.Address = supplier.Address;
        existingSupplier.City = supplier.City;
        existingSupplier.Region = supplier.Region;
        existingSupplier.PostalCode = supplier.PostalCode;
        existingSupplier.Country = supplier.Country;
        existingSupplier.Phone = supplier.Phone;
        existingSupplier.Fax = supplier.Fax;
        existingSupplier.HomePage = supplier.HomePage;

        _context.Suppliers.Update(existingSupplier);
        await _context.SaveChangesAsync();
        return existingSupplier;
    }

    public async Task<bool> DeleteSupplierAsync(int? id)
    {
        var supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierID == id);

        if (supplier == null)
        {
            return false;
        }

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();
        return true;
    }
}
