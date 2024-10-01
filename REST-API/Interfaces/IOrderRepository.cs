using REST_API.Domain;

namespace REST_API.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetAllOrdersAsync();
    Task<Order> GetOrderByIdAsync(int? id);
    Task<Order> AddOrderAsync(Order order);
    Task<Order> EditOrderAsync(Order order);
    Task<bool> DeleteOrderAsync(int? id);
}
