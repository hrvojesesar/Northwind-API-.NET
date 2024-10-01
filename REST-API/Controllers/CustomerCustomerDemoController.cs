using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using REST_API.Commands.CustomerCustomerDemo;
using REST_API.Data;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Controllers;

public class CustomerCustomerDemoController : ControllerBase
{
    private readonly ICustomerCustomerDemoRepository _customerCustomerDemoRepository;
    private readonly ApplicationDbContext _context;

    public CustomerCustomerDemoController(ICustomerCustomerDemoRepository customerCustomerDemoRepository, ApplicationDbContext context)
    {
        _customerCustomerDemoRepository = customerCustomerDemoRepository;
        _context = context;
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

        if (customerCustomerDemos == null || !customerCustomerDemos.Any())
        {
            return NotFound();
        }

        var customerIDs = customerCustomerDemos.Select(ccd => ccd.CustomerID).ToList();
        var customerTypeIDs = customerCustomerDemos.Select(ccd => ccd.CustomerTypeID).ToList();

        var customers = await _context.Customers
            .Where(c => customerIDs.Contains(c.CustomerID))
            .ToDictionaryAsync(c => c.CustomerID, c => c.CompanyName);

        var customerDemographics = await _context.CustomerDemographics
            .Where(cd => customerTypeIDs.Contains(cd.CustomerTypeID))
            .ToDictionaryAsync(cd => cd.CustomerTypeID, cd => cd.CustomerDesc);

        foreach (var customerCustomerDemo in customerCustomerDemos)
        {
            customers.TryGetValue(customerCustomerDemo.CustomerID, out var companyName);
            customerDemographics.TryGetValue(customerCustomerDemo.CustomerTypeID, out var customerDesc);

            customerCustomerDemo.CustomerName = companyName ?? "Unknown";
            customerCustomerDemo.CustomerTypeDescription = customerDesc ?? "Unknown";
        }
        return Ok(customerCustomerDemos);
    }

    /// <summary>
    /// Get CustomerCustomerDemo by IDs
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="customerTypeId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("api/GetCustomerCustomerDemoById/{customerId}/{customerTypeId}")]
    [ProducesResponseType(typeof(CustomerCustomerDemo), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCustomerCustomerDemoById(string? customerId, string? customerTypeId)
    {
        if (customerId == null || customerTypeId == null)
        {
            return BadRequest();
        }

        var customerCustomerDemo = await _customerCustomerDemoRepository.GetCustomerCustomerDemoByIdAsync(customerId, customerTypeId);
        if (customerCustomerDemo == null)
        {
            return NotFound("Customer customer demographics not found!");
        }

        var ccd = new CustomerCustomerDemo(_context)
        {
            CustomerID = customerCustomerDemo.CustomerID,
            CustomerTypeID = customerCustomerDemo.CustomerTypeID
        };
        await ccd.LoadCustomerAndCustomerTypeDetailsAsync();

        customerCustomerDemo.CustomerName = ccd.CustomerName;
        customerCustomerDemo.CustomerTypeDescription = ccd.CustomerTypeDescription;

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

    /// <summary>
    /// Delete a CustomerCustomerDemo by IDs
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="customerTypeId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("api/DeleteCustomerCustomerDemo/{customerId}/{customerTypeId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCustomerCustomerDemo(string? customerId, string? customerTypeId)
    {
        if (customerId == null || customerTypeId == null)
        {
            return BadRequest();
        }

        var isDeleted = await _customerCustomerDemoRepository.DeleteCustomerCustomerDemoAsync(customerId, customerTypeId);
        if (!isDeleted)
        {
            return NotFound("Customer customer demo not found!");
        }
        return Ok($"Customer customer demo with customerID: {customerId} and customerTypeID: {customerTypeId} deleted successfully!");
    }
}