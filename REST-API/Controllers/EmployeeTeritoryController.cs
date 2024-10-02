using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using REST_API.Commands.EmployeeTerritory;
using REST_API.Data;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeTeritoryController : ControllerBase
{
    private readonly IEmployeeTerritoryRepository _employeeTerritoryRepository;
    private readonly ApplicationDbContext _context;

    public EmployeeTeritoryController(IEmployeeTerritoryRepository employeeTerritoryRepository, ApplicationDbContext context)
    {
        _employeeTerritoryRepository = employeeTerritoryRepository;
        _context = context;
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

        if (employeeTerritories == null || !employeeTerritories.Any())
        {
            return NotFound();
        }

        var employeeIDs = employeeTerritories.Select(et => et.EmployeeID).ToList();
        var territoryIDs = employeeTerritories.Select(et => et.TerritoryID).ToList();

        var employees = await _context.Employees
            .Where(e => employeeIDs.Contains(e.EmployeeID))
            .ToDictionaryAsync(e => e.EmployeeID, e => e.FirstName + " " + e.LastName);

        var territories = await _context.Territories
            .Where(t => territoryIDs.Contains(t.TerritoryID))
            .ToDictionaryAsync(t => t.TerritoryID, t => t.TerritoryDescription.Trim());

        foreach (var employeeTerritory in employeeTerritories)
        {
            employees.TryGetValue(employeeTerritory.EmployeeID, out var employeeName);
            territories.TryGetValue(employeeTerritory.TerritoryID, out var territoryDescription);

            employeeTerritory.Employee = employeeName ?? "Unknown";
            employeeTerritory.Territory = territoryDescription ?? "Unknown";
        }

        return Ok(employeeTerritories);
    }


    /// <summary>
    /// Get EmployeeTerritory by EmployeeId and TerritoryId
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="territoryId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetEmployeeTerritoryById/{employeeId}/{territoryId}")]
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

        var et = new EmployeeTerritory(_context)
        {
            EmployeeID = employeeTerritory.EmployeeID,
            TerritoryID = employeeTerritory.TerritoryID
        };
        await et.LoadEmployeeAndTerritoryDetailsAsync();

        et.Territory = et.Territory?.Trim();

        employeeTerritory.Employee = et.Employee;
        employeeTerritory.Territory = et.Territory;

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
    [Route("DeleteEmployeeTerritory/{employeeId}/{territoryId}")]
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