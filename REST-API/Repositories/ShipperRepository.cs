using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Repositories;

public class ShipperRepository : IShipperRepository
{
    private readonly ApplicationDbContext _context;

    public ShipperRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Shipper>> GetAllShippersAsync()
    {
        return await _context.Shippers.ToListAsync();
    }

    public async Task<Shipper> GetShipperByIdAsync(int? id)
    {
        return await _context.Shippers.FirstOrDefaultAsync(s => s.ShipperID == id);
    }

    public async Task<Shipper> AddShipperAsync(Shipper shipper)
    {
        if (shipper == null)
        {
            throw new ArgumentNullException(nameof(shipper));
        }

        await _context.Shippers.AddAsync(shipper);
        await _context.SaveChangesAsync();
        return shipper;
    }

    public async Task<Shipper> EditShipperAsync(Shipper shipper)
    {
        var existingShipper = await _context.Shippers.FirstOrDefaultAsync(s => s.ShipperID == shipper.ShipperID);

        if (existingShipper == null)
        {
            return null;
        }

        existingShipper.CompanyName = shipper.CompanyName;
        existingShipper.Phone = shipper.Phone;

        _context.Shippers.Update(existingShipper);
        await _context.SaveChangesAsync();
        return existingShipper;
    }

    public async Task<bool> DeleteShipperAsync(int? id)
    {
        var shipper = await _context.Shippers.FirstOrDefaultAsync(s => s.ShipperID == id);

        if (shipper == null)
        {
            return false;
        }

        _context.Shippers.Remove(shipper);
        await _context.SaveChangesAsync();
        return true;
    }
}
