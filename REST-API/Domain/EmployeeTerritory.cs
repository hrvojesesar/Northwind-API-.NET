using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace REST_API.Domain;

public class EmployeeTerritory
{
    public int EmployeeID { get; set; }
    public string TerritoryID { get; set; }
}
