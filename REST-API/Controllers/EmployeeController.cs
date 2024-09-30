using Microsoft.AspNetCore.Mvc;
using REST_API.Commands.Employee;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    /// <summary>
    /// Get all employees
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("GetAllEmployees")]
    [ProducesResponseType(typeof(List<Employee>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await _employeeRepository.GetAllEmployeesAsync();
        if (employees == null)
        {
            return NotFound();
        }

        return Ok(employees);
    }

    /// <summary>
    /// Get employee by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetEmployeeById/{id}")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEmployeeById(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    /// <summary>
    /// Add new employee
    /// </summary>
    /// <param name="addEmployeeRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("AddEmployee")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddEmployee(AddEmployeeRequest addEmployeeRequest)
    {
        if (addEmployeeRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        ValidateDatesForAdd(addEmployeeRequest);

        var employee = new Employee
        {
            LastName = addEmployeeRequest.LastName,
            FirstName = addEmployeeRequest.FirstName,
            Title = addEmployeeRequest.Title,
            TitleOfCourtesy = addEmployeeRequest.TitleOfCourtesy,
            BirthDate = addEmployeeRequest.BirthDate,
            HireDate = addEmployeeRequest.HireDate,
            Address = addEmployeeRequest.Address,
            City = addEmployeeRequest.City,
            Region = addEmployeeRequest.Region,
            PostalCode = addEmployeeRequest.PostalCode,
            Country = addEmployeeRequest.Country,
            HomePhone = addEmployeeRequest.HomePhone,
            Extension = addEmployeeRequest.Extension,
            Photo = addEmployeeRequest.Photo,
            Notes = addEmployeeRequest.Notes,
            ReportsTo = addEmployeeRequest.ReportsTo,
            PhotoPath = addEmployeeRequest.PhotoPath,
            Salary = addEmployeeRequest.Salary
        };

        var addedEmployee = await _employeeRepository.AddEmployeeAsync(employee);
        return CreatedAtAction(nameof(GetEmployeeById), new { id = addedEmployee.EmployeeID }, addedEmployee);
    }

    /// <summary>
    /// Edit employee
    /// </summary>
    /// <param name="editEmployeeRequest"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("EditEmployee")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditEmployee(EditEmployeeRequest editEmployeeRequest)
    {
        if (editEmployeeRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        ValidateDatesForEdit(editEmployeeRequest);

        var employee = new Employee
        {
            EmployeeID = editEmployeeRequest.EmployeeID,
            LastName = editEmployeeRequest.LastName,
            FirstName = editEmployeeRequest.FirstName,
            Title = editEmployeeRequest.Title,
            TitleOfCourtesy = editEmployeeRequest.TitleOfCourtesy,
            BirthDate = editEmployeeRequest.BirthDate,
            HireDate = editEmployeeRequest.HireDate,
            Address = editEmployeeRequest.Address,
            City = editEmployeeRequest.City,
            Region = editEmployeeRequest.Region,
            PostalCode = editEmployeeRequest.PostalCode,
            Country = editEmployeeRequest.Country,
            HomePhone = editEmployeeRequest.HomePhone,
            Extension = editEmployeeRequest.Extension,
            Photo = editEmployeeRequest.Photo,
            Notes = editEmployeeRequest.Notes,
            ReportsTo = editEmployeeRequest.ReportsTo,
            PhotoPath = editEmployeeRequest.PhotoPath,
            Salary = editEmployeeRequest.Salary
        };

        var editedEmployee = await _employeeRepository.EditEmployeeAsync(employee);
        if (editedEmployee == null)
        {
            return NotFound();
        }

        return Ok(editedEmployee);
    }

    /// <summary>
    /// Delete employee
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("DeleteEmployee/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteEmployee(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var result = await _employeeRepository.DeleteEmployeeAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return Ok($"Employee with id: {id} is successfully deleted!");
    }

    private void ValidateDatesForAdd(AddEmployeeRequest addEmployeeRequest)
    {
        if (addEmployeeRequest.BirthDate != null && addEmployeeRequest.HireDate != null)
        {
            if (addEmployeeRequest.BirthDate > addEmployeeRequest.HireDate)
            {
                ModelState.AddModelError("BirthDate", "Birth date cannot be greater than hire date");
            }
        }
    }

    private void ValidateDatesForEdit(EditEmployeeRequest editEmployeeRequest)
    {
        if (editEmployeeRequest.BirthDate != null && editEmployeeRequest.HireDate != null)
        {
            if (editEmployeeRequest.BirthDate > editEmployeeRequest.HireDate)
            {
                ModelState.AddModelError("BirthDate", "Birth date cannot be greater than hire date");
            }
        }
    }
}