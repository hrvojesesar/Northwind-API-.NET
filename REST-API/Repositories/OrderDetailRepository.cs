using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Repositories;

public class OrderDetailRepository : IOrderDetailRepository
{
    private readonly ApplicationDbContext applicationDbContext;

    public OrderDetailRepository(ApplicationDbContext applicationDbContext)
    {
        this.applicationDbContext = applicationDbContext;
    }

    public async Task<List<OrderDetail>> GetAllOrderDetailsAsync()
    {
        return await applicationDbContext.OrderDetails.ToListAsync();
    }

    public async Task<OrderDetail> GetOrderDetailByIdAsync(int? orderId, int? productId)
    {
        return await applicationDbContext.OrderDetails
            .FirstOrDefaultAsync(od => od.OrderID == orderId && od.ProductID == productId);
    }

    public async Task<OrderDetail> AddOrderDetailAsync(OrderDetail orderDetail)
    {
        if (orderDetail == null)
        {
            throw new ArgumentNullException(nameof(orderDetail));
        }

        await applicationDbContext.OrderDetails.AddAsync(orderDetail);
        await applicationDbContext.SaveChangesAsync();
        return orderDetail;
    }

    public async Task<OrderDetail> EditOrderDetailAsync(OrderDetail orderDetail)
    {
        var existingOrderDetail = await applicationDbContext.OrderDetails
            .FirstOrDefaultAsync(od => od.OrderID == orderDetail.OrderID && od.ProductID == orderDetail.ProductID);

        if (existingOrderDetail == null)
        {
            return null;
        }

        existingOrderDetail.UnitPrice = orderDetail.UnitPrice;
        existingOrderDetail.Quantity = orderDetail.Quantity;
        existingOrderDetail.Discount = orderDetail.Discount;

        applicationDbContext.OrderDetails.Update(existingOrderDetail);
        await applicationDbContext.SaveChangesAsync();
        return orderDetail;
    }

    public async Task<bool> DeleteOrderDetailAsync(int? orderId, int? productId)
    {
        var orderDetail = await applicationDbContext.OrderDetails
            .FirstOrDefaultAsync(od => od.OrderID == orderId && od.ProductID == productId);

        if (orderDetail == null)
        {
            return false;
        }

        applicationDbContext.OrderDetails.Remove(orderDetail);
        await applicationDbContext.SaveChangesAsync();
        return true;
    }
}