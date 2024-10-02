using REST_API.Domain;

namespace REST_API.Interfaces;

public interface IOrderDetailRepository
{
    Task<List<OrderDetail>> GetAllOrderDetailsAsync();
    Task<OrderDetail> GetOrderDetailByIdAsync(int? orderId, int? productId);
    Task<OrderDetail> AddOrderDetailAsync(OrderDetail orderDetail);
    Task<OrderDetail> EditOrderDetailAsync(OrderDetail orderDetail);
    Task<bool> DeleteOrderDetailAsync(int? orderId, int? productId);
}