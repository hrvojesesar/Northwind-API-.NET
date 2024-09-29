using Microsoft.AspNetCore.Mvc;
using REST_API.Commands.CustomerDemographic;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerDemographicController : ControllerBase
{
    private readonly ICustomerDemographicRepository customerDemographicRepository;

    public CustomerDemographicController(ICustomerDemographicRepository customerDemographicRepository)
    {
        this.customerDemographicRepository = customerDemographicRepository;
    }

    /// <summary>
    /// Get all customer demographics
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("GetAllCustomerDemographics")]
    [ProducesResponseType(typeof(List<CustomerDemographic>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllCustomerDemographics()
    {
        var customerDemographics = await customerDemographicRepository.GetAllCustomerDemographicsAsync();

        if (customerDemographics == null)
        {
            return NotFound();
        }

        return Ok(customerDemographics);
    }

    /// <summary>
    /// Get customer demographic by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetCustomerDemographicById/{id}")]
    [ProducesResponseType(typeof(CustomerDemographic), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCustomerDemographicById(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest();
        }

        var customerDemographic = await customerDemographicRepository.GetCustomerDemographicByIdAsync(id);

        if (customerDemographic == null)
        {
            return NotFound();
        }

        return Ok(customerDemographic);
    }

    /// <summary>
    /// Add new customer demographic
    /// </summary>
    /// <param name="addCustomerDemographicRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("AddCustomerDemographic")]
    [ProducesResponseType(typeof(CustomerDemographic), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddCustomerDemographic(AddCustomerDemographicRequest addCustomerDemographicRequest)
    {
        if (addCustomerDemographicRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingCustomerDemographic = await customerDemographicRepository.GetCustomerDemographicByIdAsync(addCustomerDemographicRequest.CustomerTypeID);

        if (existingCustomerDemographic != null)
        {
            return BadRequest("Customer demographic already exists.");
        }

        var customerDemographic = new CustomerDemographic
        {
            CustomerTypeID = addCustomerDemographicRequest.CustomerTypeID,
            CustomerDesc = addCustomerDemographicRequest.CustomerDesc
        };

        var addedCustomerDemographic = await customerDemographicRepository.AddCustomerDemographicAsync(customerDemographic);
        return CreatedAtAction(nameof(GetCustomerDemographicById), new { id = addedCustomerDemographic.CustomerTypeID }, addedCustomerDemographic);
    }

    /// <summary>
    /// Edit customer demographic
    /// </summary>
    /// <param name="editCustomerDemographicRequest"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("EditCustomerDemographic")]
    [ProducesResponseType(typeof(CustomerDemographic), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditCustomerDemographic(EditCustomerDemographicRequest editCustomerDemographicRequest)
    {
        if (editCustomerDemographicRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var customerDemographic = new CustomerDemographic
        {
            CustomerTypeID = editCustomerDemographicRequest.CustomerTypeID,
            CustomerDesc = editCustomerDemographicRequest.CustomerDesc
        };

        var editedCustomerDemographic = await customerDemographicRepository.EditCustomerDemographicAsync(customerDemographic);

        if (editedCustomerDemographic == null)
        {
            return NotFound();
        }

        return Ok(editedCustomerDemographic);
    }

    /// <summary>
    /// Delete customer demographic by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("DeleteCustomerDemographic/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCustomerDemographic(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest();
        }

        var isDeleted = await customerDemographicRepository.DeleteCustomerDemographicAsync(id);

        if (!isDeleted)
        {
            return NotFound();
        }

        return Ok($"Customer demographic with id: {id} is successfully deleted!");
    }

}
