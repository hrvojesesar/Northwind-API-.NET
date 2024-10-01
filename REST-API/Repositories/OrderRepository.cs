using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int? id)
    {
        return await _context.Orders.FirstOrDefaultAsync(o => o.OrderID == id);
    }

    public async Task<Order> AddOrderAsync(Order order)
    {
        if (order == null)
        {
            throw new ArgumentNullException(nameof(order));
        }

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> EditOrderAsync(Order order)
    {
        var existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.OrderID == order.OrderID);

        if (existingOrder == null)
        {
            return null;
        }

        existingOrder.CustomerID = order.CustomerID;
        existingOrder.EmployeeID = order.EmployeeID;
        existingOrder.OrderDate = order.OrderDate;
        existingOrder.RequiredDate = order.RequiredDate;
        existingOrder.ShipVia = order.ShipVia;
        existingOrder.Freight = order.Freight;
        existingOrder.ShipName = order.ShipName;
        existingOrder.ShipAddress = order.ShipAddress;
        existingOrder.ShipCity = order.ShipCity;
        existingOrder.ShipRegion = order.ShipRegion;
        existingOrder.ShipPostalCode = order.ShipPostalCode;
        existingOrder.ShipCountry = order.ShipCountry;

        _context.Orders.Update(existingOrder);
        await _context.SaveChangesAsync();
        return existingOrder;
    }

    public async Task<bool> DeleteOrderAsync(int? id)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderID == id);

        if (order == null)
        {
            return false;
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }
}