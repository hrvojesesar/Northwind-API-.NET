using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace REST_API.Domain;

public class OrderDetail
{
    private readonly ApplicationDbContext _context;

    public OrderDetail() { }

    public OrderDetail(ApplicationDbContext context)
    {
        _context = context;
    }

    public int OrderID { get; set; }
    [NotMapped]
    public string? OrderName { get; set; }
    public int ProductID { get; set; }
    [NotMapped]
    public string? ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public short Quantity { get; set; }
    public float Discount { get; set; }

    public async Task LoadOrderAndProductDetailsAsync()
    {
        OrderName = await _context.Orders
            .Where(x => x.OrderID == OrderID)
            .Select(x => x.ShipName)
            .FirstOrDefaultAsync();

        ProductName = await _context.Products
            .Where(x => x.ProductID == ProductID)
            .Select(x => x.ProductName)
            .FirstOrDefaultAsync();
    }
}
