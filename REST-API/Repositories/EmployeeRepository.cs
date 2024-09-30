using Microsoft.EntityFrameworkCore;
using REST_API.Data;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ApplicationDbContext _context;

    public EmployeeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        return await _context.Employees.ToListAsync();
    }

    public async Task<Employee> GetEmployeeByIdAsync(int? id)
    {
        return await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeID == id);
    }

    public async Task<Employee> AddEmployeeAsync(Employee employee)
    {
        if (employee == null)
        {
            throw new ArgumentNullException(nameof(employee));
        }

        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<Employee> EditEmployeeAsync(Employee employee)
    {
        var existingEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeID == employee.EmployeeID);

        if (existingEmployee == null)
        {
            return null;
        }

        existingEmployee.LastName = employee.LastName;
        existingEmployee.FirstName = employee.FirstName;
        existingEmployee.Title = employee.Title;
        existingEmployee.TitleOfCourtesy = employee.TitleOfCourtesy;
        existingEmployee.BirthDate = employee.BirthDate;
        existingEmployee.HireDate = employee.HireDate;
        existingEmployee.Address = employee.Address;
        existingEmployee.City = employee.City;
        existingEmployee.Region = employee.Region;
        existingEmployee.PostalCode = employee.PostalCode;
        existingEmployee.Country = employee.Country;
        existingEmployee.HomePhone = employee.HomePhone;
        existingEmployee.Extension = employee.Extension;
        existingEmployee.Photo = employee.Photo;
        existingEmployee.Notes = employee.Notes;
        existingEmployee.ReportsTo = employee.ReportsTo;
        existingEmployee.PhotoPath = employee.PhotoPath;
        existingEmployee.Salary = employee.Salary;

        _context.Employees.Update(existingEmployee);
        await _context.SaveChangesAsync();
        return existingEmployee;
    }

    public async Task<bool> DeleteEmployeeAsync(int? id)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeID == id);
        if (employee == null)
        {
            return false;
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return true;
    }


}
