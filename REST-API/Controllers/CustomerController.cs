using Microsoft.AspNetCore.Mvc;
using REST_API.Commands.Customer;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// Get all customers
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("GetAllCustomers")]
    [ProducesResponseType(typeof(List<Customer>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Customer>>> GetAllCustomers()
    {
        var customers = await _customerRepository.GetAllCustomersAsync();

        if (customers == null)
        {
            return NotFound();
        }

        return Ok(customers);
    }

    /// <summary>
    /// Get customer by id
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetCustomerById")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Customer>> GetCustomerById(string? customerId)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(customerId);

        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    /// <summary>
    /// Add new customer
    /// </summary>
    /// <param name="addCustomerRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("AddCustomer")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Customer>> AddCustomer(AddCustomerRequest addCustomerRequest)
    {
        if (addCustomerRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingCustomer = await _customerRepository.GetCustomerByIdAsync(addCustomerRequest.CustomerID);

        if (existingCustomer != null)
        {
            return BadRequest("Customer already exists");
        }

        var customer = new Customer
        {
            CustomerID = addCustomerRequest.CustomerID,
            CompanyName = addCustomerRequest.CompanyName,
            ContactName = addCustomerRequest.ContactName,
            ContactTitle = addCustomerRequest.ContactTitle,
            Address = addCustomerRequest.Address,
            City = addCustomerRequest.City,
            Region = addCustomerRequest.Region,
            PostalCode = addCustomerRequest.PostalCode,
            Country = addCustomerRequest.Country,
            Phone = addCustomerRequest.Phone,
            Fax = addCustomerRequest.Fax
        };

        var addedCustomer = await _customerRepository.AddCustomerAsync(customer);
        return CreatedAtAction(nameof(GetCustomerById), new { customerId = addedCustomer.CustomerID }, addedCustomer);
    }

    /// <summary>
    /// Edit customer
    /// </summary>
    /// <param name="editCustomerRequest"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("EditCustomer")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Customer>> EditCustomer(EditCustomerRequest editCustomerRequest)
    {
        if (editCustomerRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingCustomer = await _customerRepository.GetCustomerByIdAsync(editCustomerRequest.CustomerID);

        if (existingCustomer == null)
        {
            return BadRequest("Customer does not exist");
        }

        var customer = new Customer
        {
            CustomerID = editCustomerRequest.CustomerID,
            CompanyName = editCustomerRequest.CompanyName,
            ContactName = editCustomerRequest.ContactName,
            ContactTitle = editCustomerRequest.ContactTitle,
            Address = editCustomerRequest.Address,
            City = editCustomerRequest.City,
            Region = editCustomerRequest.Region,
            PostalCode = editCustomerRequest.PostalCode,
            Country = editCustomerRequest.Country,
            Phone = editCustomerRequest.Phone,
            Fax = editCustomerRequest.Fax
        };

        var editedCustomer = await _customerRepository.EditCustomerAsync(customer);

        if (editedCustomer == null)
        {
            return NotFound();
        }

        return Ok(editedCustomer);
    }

    /// <summary>
    /// Delete customer
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("DeleteCustomer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteCustomer(string? customerId)
    {
        if (string.IsNullOrWhiteSpace(customerId))
        {
            return BadRequest();
        }

        var result = await _customerRepository.DeleteCustomerAsync(customerId);

        if (!result)
        {
            return NotFound();
        }

        return Ok($"Customer with id: {customerId} is successfully deleted!");
    }
}

