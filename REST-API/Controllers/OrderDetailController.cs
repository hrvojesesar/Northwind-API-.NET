using Microsoft.AspNetCore.Mvc;
using REST_API.Commands.OrderDetail;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderDetailController : ControllerBase
{
    private readonly IOrderDetailRepository orderDetailRepository;

    public OrderDetailController(IOrderDetailRepository orderDetailRepository)
    {
        this.orderDetailRepository = orderDetailRepository;
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
        return Ok(orderDetails);
    }

    /// <summary>
    /// Get order detail by order id and product id
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetOrderDetailById")]
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
    [Route("DeleteOrderDetail")]
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
