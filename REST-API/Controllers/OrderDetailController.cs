using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using REST_API.Commands.OrderDetail;
using REST_API.Data;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderDetailController : ControllerBase
{
    private readonly IOrderDetailRepository orderDetailRepository;
    private readonly ApplicationDbContext _context;

    public OrderDetailController(IOrderDetailRepository orderDetailRepository, ApplicationDbContext context)
    {
        this.orderDetailRepository = orderDetailRepository;
        _context = context;
    }

    /// <summary>
    /// Get all order details
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("GetAllOrderDetails")]
    [ProducesResponseType(typeof(List<OrderDetail>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllOrderDetails()
    {
        var orderDetails = await orderDetailRepository.GetAllOrderDetailsAsync();

        if (orderDetails == null || !orderDetails.Any())
        {
            return NotFound();
        }

        var orderIDs = orderDetails.Select(od => od.OrderID).ToList();
        var productIDs = orderDetails.Select(od => od.ProductID).ToList();

        var orders = await _context.Orders
             .Where(o => orderIDs.Contains(o.OrderID))
             .ToDictionaryAsync(o => o.OrderID, o => o.ShipName);

        var products = await _context.Products
            .Where(p => productIDs.Contains(p.ProductID))
            .ToDictionaryAsync(p => p.ProductID, p => p.ProductName);

        foreach (var orderDetail in orderDetails)
        {
            orders.TryGetValue(orderDetail.OrderID, out var shipName);
            products.TryGetValue(orderDetail.ProductID, out var productName);

            orderDetail.OrderName = shipName ?? "Unknown";
            orderDetail.ProductName = productName ?? "Unknown";
        }

        return Ok(orderDetails);
    }

    /// <summary>
    /// Get order detail by order id and product id
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetOrderDetailById/{orderId}/{productId}")]
    [ProducesResponseType(typeof(OrderDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOrderDetailById(int? orderId, int? productId)
    {
        if (orderId == null || productId == null)
        {
            return BadRequest();
        }

        var orderDetail = await orderDetailRepository.GetOrderDetailByIdAsync(orderId, productId);
        if (orderDetail == null)
        {
            return NotFound("Order detail not found!");
        }

        var od = new OrderDetail(_context)
        {
            OrderID = orderDetail.OrderID,
            ProductID = orderDetail.ProductID,
            UnitPrice = orderDetail.UnitPrice,
            Quantity = orderDetail.Quantity,
            Discount = orderDetail.Discount
        };
        await od.LoadOrderAndProductDetailsAsync();

        orderDetail.OrderName = od.OrderName;
        orderDetail.ProductName = od.ProductName;

        return Ok(orderDetail);
    }

    /// <summary>
    /// Add new order detail
    /// </summary>
    /// <param name="addOrderDetailRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("AddOrderDetail")]
    [ProducesResponseType(typeof(OrderDetail), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddOrderDetail(AddOrderDetailRequest addOrderDetailRequest)
    {
        if (addOrderDetailRequest.OrderID == null || addOrderDetailRequest.ProductID == null)
        {
            return BadRequest();
        }

        if (addOrderDetailRequest == null)
        {
            return BadRequest();
        }

        var existingOrderDetail = await orderDetailRepository.GetOrderDetailByIdAsync(addOrderDetailRequest.OrderID, addOrderDetailRequest.ProductID);
        if (existingOrderDetail != null)
        {
            return BadRequest("Order detail already exists!");
        }

        var orderDetail = new OrderDetail
        {
            OrderID = addOrderDetailRequest.OrderID,
            ProductID = addOrderDetailRequest.ProductID,
            UnitPrice = addOrderDetailRequest.UnitPrice,
            Quantity = addOrderDetailRequest.Quantity,
            Discount = addOrderDetailRequest.Discount
        };

        var addedOrderDetail = await orderDetailRepository.AddOrderDetailAsync(orderDetail);
        return CreatedAtAction(nameof(GetOrderDetailById), new { orderId = addedOrderDetail.OrderID, productId = addedOrderDetail.ProductID }, addedOrderDetail);
    }

    /// <summary>
    /// Edit order detail
    /// </summary>
    /// <param name="editOrderDetailRequest"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("EditOrderDetail")]
    [ProducesResponseType(typeof(OrderDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditOrderDetail(EditOrderDetailRequest editOrderDetailRequest)
    {
        if (editOrderDetailRequest.OrderID == null || editOrderDetailRequest.ProductID == null)
        {
            return BadRequest();
        }

        if (editOrderDetailRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingOrderDetail = await orderDetailRepository.GetOrderDetailByIdAsync(editOrderDetailRequest.OrderID, editOrderDetailRequest.ProductID);
        if (existingOrderDetail == null)
        {
            return NotFound("Order detail not found!");
        }

        var orderDetail = new OrderDetail
        {
            OrderID = editOrderDetailRequest.OrderID,
            ProductID = editOrderDetailRequest.ProductID,
            UnitPrice = editOrderDetailRequest.UnitPrice,
            Quantity = editOrderDetailRequest.Quantity,
            Discount = editOrderDetailRequest.Discount
        };

        var editedOrderDetail = await orderDetailRepository.EditOrderDetailAsync(orderDetail);

        if (editedOrderDetail == null)
        {
            return NotFound("Order detail not found!");
        }

        return Ok(editedOrderDetail);
    }

    /// <summary>
    /// Delete order detail
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("DeleteOrderDetail/{orderId}/{productId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteOrderDetail(int? orderId, int? productId)
    {
        if (orderId == null || productId == null)
        {
            return BadRequest();
        }

        var isDeleted = await orderDetailRepository.DeleteOrderDetailAsync(orderId, productId);
        if (!isDeleted)
        {
            return NotFound("Order detail not found!");
        }

        return Ok($"Order detail wih order id: {orderId} and product id: {productId} is successfully deleted!");
    }
}