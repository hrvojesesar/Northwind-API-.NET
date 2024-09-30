using Microsoft.AspNetCore.Mvc;
using REST_API.Commands.CustomerCustomerDemo;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Controllers;

public class CustomerCustomerDemoController : ControllerBase
{
    private readonly ICustomerCustomerDemoRepository _customerCustomerDemoRepository;

    public CustomerCustomerDemoController(ICustomerCustomerDemoRepository customerCustomerDemoRepository)
    {
        _customerCustomerDemoRepository = customerCustomerDemoRepository;
    }

    /// <summary>
    /// Get all CustomerCustomerDemos
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("api/GetAllCustomerCustomerDemos")]
    [ProducesResponseType(typeof(List<CustomerCustomerDemo>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllCustomerCustomerDemos()
    {
        var customerCustomerDemos = await _customerCustomerDemoRepository.GetAllCustomerCustomerDemosAsync();
        return Ok(customerCustomerDemos);
    }

    /// <summary>
    /// Get CustomerCustomerDemo by IDs
    /// </summary>
    /// <param name="customerID"></param>
    /// <param name="customerTypeID"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("api/GetCustomerCustomerDemoById")]
    [ProducesResponseType(typeof(CustomerCustomerDemo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCustomerCustomerDemoById(string? customerID, string? customerTypeID)
    {
        if (customerID == null || customerTypeID == null)
        {
            return BadRequest();
        }

        var customerCustomerDemo = await _customerCustomerDemoRepository.GetCustomerCustomerDemoByIdAsync(customerID, customerTypeID);
        if (customerCustomerDemo == null)
        {
            return NotFound("Customer customer demographics not found!");
        }
        return Ok(customerCustomerDemo);
    }

    /// <summary>
    /// Add a new CustomerCustomerDemo
    /// </summary>
    /// <param name="addCustomerCustomerDemoRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("api/AddCustomerCustomerDemo")]
    [ProducesResponseType(typeof(CustomerCustomerDemo), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddCustomerCustomerDemo(AddCustomerCustomerDemoRequest addCustomerCustomerDemoRequest)
    {
        if (addCustomerCustomerDemoRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingCustomerCustomerDemo = await _customerCustomerDemoRepository.GetCustomerCustomerDemoByIdAsync(addCustomerCustomerDemoRequest.CustomerID, addCustomerCustomerDemoRequest.CustomerTypeID);

        if (existingCustomerCustomerDemo != null)
        {
            return BadRequest("Customer customer demo already exists.");
        }

        var customerCustomerDemo = new CustomerCustomerDemo
        {
            CustomerID = addCustomerCustomerDemoRequest.CustomerID,
            CustomerTypeID = addCustomerCustomerDemoRequest.CustomerTypeID
        };

        var addedCustomerCustomerDemo = await _customerCustomerDemoRepository.AddCustomerCustomerDemoAsync(customerCustomerDemo);
        return CreatedAtAction(nameof(GetCustomerCustomerDemoById), new { customerID = addedCustomerCustomerDemo.CustomerID, customerTypeID = addedCustomerCustomerDemo.CustomerTypeID }, addedCustomerCustomerDemo);
    }
}
