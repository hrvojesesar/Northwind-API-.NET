using Microsoft.AspNetCore.Mvc;
using REST_API.Commands.Territory;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TerritoryController : ControllerBase
{
    private readonly ITerritoryRepository _territoryRepository;

    public TerritoryController(ITerritoryRepository territoryRepository)
    {
        _territoryRepository = territoryRepository;
    }

    /// <summary>
    /// Get all territories
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("GetAllTerritories")]
    [ProducesResponseType(typeof(List<Territory>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllTerritories()
    {
        var territories = await _territoryRepository.GetAllTerritoriesAsync();
        if (territories == null)
        {
            return NotFound();
        }
        return Ok(territories);
    }

    /// <summary>
    /// Get territory by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetTerritoryById/{id}")]
    [ProducesResponseType(typeof(Territory), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTerritoryById(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }

        var territory = await _territoryRepository.GetTerritoryByIdAsync(id);
        if (territory == null)
        {
            return NotFound();
        }
        return Ok(territory);
    }

    /// <summary>
    /// Add new territory
    /// </summary>
    /// <param name="addTerritoryRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("AddTerritory")]
    [ProducesResponseType(typeof(Territory), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddTerritory(AddTerritoryRequest addTerritoryRequest)
    {
        if (addTerritoryRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingTerritory = await _territoryRepository.GetTerritoryByIdAsync(addTerritoryRequest.TerritoryID);
        if (existingTerritory != null)
        {
            return BadRequest("Territory already exists");
        }

        var territory = new Territory
        {
            TerritoryID = addTerritoryRequest.TerritoryID,
            TerritoryDescription = addTerritoryRequest.TerritoryDescription,
            RegionID = addTerritoryRequest.RegionID
        };

        var addedTerritory = await _territoryRepository.AddTerritoryAsync(territory);
        return CreatedAtAction(nameof(GetTerritoryById), new { id = addedTerritory.TerritoryID }, addedTerritory);
    }

    /// <summary>
    /// Edit territory
    /// </summary>
    /// <param name="editTerritoryRequest"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("EditTerritory")]
    [ProducesResponseType(typeof(Territory), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditTerritory(EditTerritoryRequest editTerritoryRequest)
    {
        if (editTerritoryRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingTerritory = await _territoryRepository.GetTerritoryByIdAsync(editTerritoryRequest.TerritoryID);
        if (existingTerritory == null)
        {
            return NotFound();
        }

        var territory = new Territory
        {
            TerritoryID = editTerritoryRequest.TerritoryID,
            TerritoryDescription = editTerritoryRequest.TerritoryDescription,
            RegionID = editTerritoryRequest.RegionID
        };

        var editedTerritory = await _territoryRepository.EditTerritoryAsync(territory);

        if (editedTerritory == null)
        {
            return BadRequest();
        }

        return Ok(editedTerritory);
    }

    /// <summary>
    /// Delete territory by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("DeleteTerritory/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteTerritory(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }

        var isDeleted = await _territoryRepository.DeleteTerritoryAsync(id);
        if (!isDeleted)
        {
            return NotFound();
        }

        return Ok($"Territory with id: {id} is successfully deleted!");
    }
}