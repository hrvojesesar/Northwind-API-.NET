using Microsoft.AspNetCore.Mvc;
using REST_API.Commands.Order;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;

    public OrderController(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// Get all orders
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("GetAllOrders")]
    [ProducesResponseType(typeof(List<Order>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();
        if (orders == null)
        {
            return NotFound();
        }
        return Ok(orders);
    }

    /// <summary>
    /// Get order by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetOrderById/{id}")]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOrderById(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var order = await _orderRepository.GetOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    /// <summary>
    /// Add new order
    /// </summary>
    /// <param name="addOrderRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("AddOrder")]
    [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddOrder(AddOrderRequest addOrderRequest)
    {
        if (addOrderRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var order = new Order
        {
            CustomerID = addOrderRequest.CustomerID,
            EmployeeID = addOrderRequest.EmployeeID,
            OrderDate = addOrderRequest.OrderDate,
            RequiredDate = addOrderRequest.RequiredDate,
            ShipVia = addOrderRequest.ShipVia,
            Freight = addOrderRequest.Freight,
            ShipName = addOrderRequest.ShipName,
            ShipAddress = addOrderRequest.ShipAddress,
            ShipCity = addOrderRequest.ShipCity,
            ShipRegion = addOrderRequest.ShipRegion,
            ShipPostalCode = addOrderRequest.ShipPostalCode,
            ShipCountry = addOrderRequest.ShipCountry
        };

        var addedOrder = await _orderRepository.AddOrderAsync(order);
        return CreatedAtAction(nameof(GetOrderById), new { id = addedOrder.OrderID }, addedOrder);
    }

    /// <summary>
    /// Edit order
    /// </summary>
    /// <param name="editOrderRequest"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("EditOrder")]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditOrder(EditOrderRequest editOrderRequest)
    {
        if (editOrderRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var order = new Order
        {
            OrderID = editOrderRequest.OrderID,
            CustomerID = editOrderRequest.CustomerID,
            EmployeeID = editOrderRequest.EmployeeID,
            OrderDate = editOrderRequest.OrderDate,
            RequiredDate = editOrderRequest.RequiredDate,
            ShipVia = editOrderRequest.ShipVia,
            Freight = editOrderRequest.Freight,
            ShipName = editOrderRequest.ShipName,
            ShipAddress = editOrderRequest.ShipAddress,
            ShipCity = editOrderRequest.ShipCity,
            ShipRegion = editOrderRequest.ShipRegion,
            ShipPostalCode = editOrderRequest.ShipPostalCode,
            ShipCountry = editOrderRequest.ShipCountry
        };

        var editedOrder = await _orderRepository.EditOrderAsync(order);

        if (editedOrder == null)
        {
            return NotFound();
        }

        return Ok(editedOrder);
    }

    /// <summary>
    /// Delete order by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("DeleteOrder/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteOrder(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var result = await _orderRepository.DeleteOrderAsync(id);

        if (!result)
        {
            return NotFound();
        }

        return Ok($"Order with id: {id} is successfully deleted!");
    }
}