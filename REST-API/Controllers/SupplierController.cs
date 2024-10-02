using Microsoft.AspNetCore.Mvc;
using REST_API.Commands.Supplier;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SupplierController : ControllerBase
{
    private readonly ISupplierRepository _supplierRepository;

    public SupplierController(ISupplierRepository supplierRepository)
    {
        _supplierRepository = supplierRepository;
    }

    /// <summary>
    /// Get all suppliers
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("GetAllSuppliers")]
    [ProducesResponseType(typeof(List<Supplier>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllSuppliers()
    {
        var suppliers = await _supplierRepository.GetAllSuppliersAsync();
        return Ok(suppliers);
    }

    /// <summary>
    /// Get supplier by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetSupplierById/{id}")]
    [ProducesResponseType(typeof(Supplier), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSupplierById(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var supplier = await _supplierRepository.GetSupplierByIdAsync(id);
        if (supplier == null)
        {
            return NotFound();
        }
        return Ok(supplier);
    }

    /// <summary>
    /// Add new supplier
    /// </summary>
    /// <param name="addSupplierRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("AddSupplier")]
    [ProducesResponseType(typeof(Supplier), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddSupplier(AddSupplierRequest addSupplierRequest)
    {
        if (addSupplierRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var supplier = new Supplier
        {
            CompanyName = addSupplierRequest.CompanyName,
            ContactName = addSupplierRequest.ContactName,
            ContactTitle = addSupplierRequest.ContactTitle,
            Address = addSupplierRequest.Address,
            City = addSupplierRequest.City,
            Region = addSupplierRequest.Region,
            PostalCode = addSupplierRequest.PostalCode,
            Country = addSupplierRequest.Country,
            Phone = addSupplierRequest.Phone,
            Fax = addSupplierRequest.Fax,
            HomePage = addSupplierRequest.HomePage
        };

        var addedSupplier = await _supplierRepository.AddSupplierAsync(supplier);
        return CreatedAtAction(nameof(GetSupplierById), new { id = addedSupplier.SupplierID }, addedSupplier);
    }

    /// <summary>
    /// Edit supplier
    /// </summary>
    /// <param name="editSupplierRequest"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("EditSupplier")]
    [ProducesResponseType(typeof(Supplier), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditSupplier(EditSupplierRequest editSupplierRequest)
    {
        if (editSupplierRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var supplier = new Supplier
        {
            SupplierID = editSupplierRequest.SupplierID,
            CompanyName = editSupplierRequest.CompanyName,
            ContactName = editSupplierRequest.ContactName,
            ContactTitle = editSupplierRequest.ContactTitle,
            Address = editSupplierRequest.Address,
            City = editSupplierRequest.City,
            Region = editSupplierRequest.Region,
            PostalCode = editSupplierRequest.PostalCode,
            Country = editSupplierRequest.Country,
            Phone = editSupplierRequest.Phone,
            Fax = editSupplierRequest.Fax,
            HomePage = editSupplierRequest.HomePage
        };

        var editedSupplier = await _supplierRepository.EditSupplierAsync(supplier);

        if (editedSupplier == null)
        {
            return NotFound();
        }

        return Ok(editedSupplier);
    }

    /// <summary>
    /// Delete supplier by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("DeleteSupplier/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteSupplier(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var isDeleted = await _supplierRepository.DeleteSupplierAsync(id);

        if (!isDeleted)
        {
            return NotFound();
        }

        return Ok($"Supplier with id: {id} is successfully deleted!");
    }
}