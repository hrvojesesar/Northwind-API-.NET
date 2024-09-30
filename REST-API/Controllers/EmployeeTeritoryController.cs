using Microsoft.AspNetCore.Mvc;
using REST_API.Commands.EmployeeTerritory;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeTeritoryController : ControllerBase
{
    private readonly IEmployeeTerritoryRepository _employeeTerritoryRepository;

    public EmployeeTeritoryController(IEmployeeTerritoryRepository employeeTerritoryRepository)
    {
        _employeeTerritoryRepository = employeeTerritoryRepository;
    }

    /// <summary>
    /// Get all EmployeeTerritories
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("GetAllEmployeeTerritories")]
    [ProducesResponseType(typeof(List<EmployeeTerritory>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllEmployeeTerritories()
    {
        var employeeTerritories = await _employeeTerritoryRepository.GetAllEmployeeTerritoriesAsync();
        return Ok(employeeTerritories);
    }
    /// <summary>
    /// Get EmployeeTerritory by EmployeeId and TerritoryId
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="territoryId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetEmployeeTerritoryById")]
    [ProducesResponseType(typeof(EmployeeTerritory), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEmployeeTerritoryById(int? employeeId, string? territoryId)
    {
        if (employeeId == null || string.IsNullOrEmpty(territoryId))
        {
            return BadRequest();
        }

        var employeeTerritory = await _employeeTerritoryRepository.GetEmployeeTerritoryByIdAsync(employeeId, territoryId);

        if (employeeTerritory == null)
        {
            return NotFound();
        }

        return Ok(employeeTerritory);
    }

    /// <summary>
    /// Add EmployeeTerritory
    /// </summary>
    /// <param name="addEmployeeTerritoryRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("AddEmployeeTerritory")]
    [ProducesResponseType(typeof(EmployeeTerritory), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddEmployeeTerritory(AddEmployeeTerritoryRequest addEmployeeTerritoryRequest)
    {
        if (addEmployeeTerritoryRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingEmployeeTerritory = await _employeeTerritoryRepository.GetEmployeeTerritoryByIdAsync(addEmployeeTerritoryRequest.EmployeeID, addEmployeeTerritoryRequest.TerritoryID);

        if (existingEmployeeTerritory != null)
        {
            return BadRequest("EmployeeTerritory already exists");
        }

        var employeeTerritory = new EmployeeTerritory
        {
            EmployeeID = addEmployeeTerritoryRequest.EmployeeID,
            TerritoryID = addEmployeeTerritoryRequest.TerritoryID
        };

        var addedEmployeeTerritory = await _employeeTerritoryRepository.AddEmployeeTerritoryAsync(employeeTerritory);
        return CreatedAtAction(nameof(GetEmployeeTerritoryById), new { employeeId = addedEmployeeTerritory.EmployeeID, territoryId = addedEmployeeTerritory.TerritoryID }, addedEmployeeTerritory);
    }

    /// <summary>
    /// Delete EmployeeTerritory by EmployeeId and TerritoryId
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="territoryId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("DeleteEmployeeTerritory")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteEmployeeTerritory(int? employeeId, string? territoryId)
    {
        if (employeeId == null || string.IsNullOrEmpty(territoryId))
        {
            return BadRequest();
        }

        var isDeleted = await _employeeTerritoryRepository.DeleteEmployeeTerritoryAsync(employeeId, territoryId);

        if (!isDeleted)
        {
            return NotFound();
        }

        return Ok($"Employee Territory with employeeId: {employeeId} and territoryId: {territoryId} deleted successfully");
    }

}
