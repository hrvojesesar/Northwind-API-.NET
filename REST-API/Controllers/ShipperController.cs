using Microsoft.AspNetCore.Mvc;
using REST_API.Commands.Shipper;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShipperController : ControllerBase
{
    private readonly IShipperRepository _shipperRepository;

    public ShipperController(IShipperRepository shipperRepository)
    {
        _shipperRepository = shipperRepository;
    }

    /// <summary>
    /// Get all shippers
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("GetAllShippers")]
    [ProducesResponseType(typeof(List<Shipper>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllShippers()
    {
        var shippers = await _shipperRepository.GetAllShippersAsync();

        if (shippers == null)
        {
            return NotFound();
        }
        return Ok(shippers);
    }

    /// <summary>
    /// Get shipper by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetShipperById/{id}")]
    [ProducesResponseType(typeof(Shipper), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetShipperById(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var shipper = await _shipperRepository.GetShipperByIdAsync(id);

        if (shipper == null)
        {
            return NotFound();
        }

        return Ok(shipper);
    }

    /// <summary>
    /// Add new shipper
    /// </summary>
    /// <param name="addShipperRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("AddShipper")]
    [ProducesResponseType(typeof(Shipper), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddShipper(AddShipperRequest addShipperRequest)
    {
        if (addShipperRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var shipper = new Shipper
        {
            CompanyName = addShipperRequest.CompanyName,
            Phone = addShipperRequest.Phone
        };

        var addedShipper = await _shipperRepository.AddShipperAsync(shipper);
        return CreatedAtAction(nameof(GetShipperById), new { id = addedShipper.ShipperID }, addedShipper);
    }

    /// <summary>
    /// Edit shipper
    /// </summary>
    /// <param name="editShipperRequest"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("EditShipper")]
    [ProducesResponseType(typeof(Shipper), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditShipper(EditShipperRequest editShipperRequest)
    {
        if (editShipperRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var shipper = new Shipper
        {
            ShipperID = editShipperRequest.ShipperID,
            CompanyName = editShipperRequest.CompanyName,
            Phone = editShipperRequest.Phone
        };

        var editedShipper = await _shipperRepository.EditShipperAsync(shipper);

        if (editedShipper == null)
        {
            return NotFound();
        }

        return Ok(editedShipper);
    }

    /// <summary>
    /// Delete shipper by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("DeleteShipper/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteShipper(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var isDeleted = await _shipperRepository.DeleteShipperAsync(id);

        if (!isDeleted)
        {
            return NotFound();
        }

        return Ok($"Shipper with id: {id} is successfully deleted!");
    }
}