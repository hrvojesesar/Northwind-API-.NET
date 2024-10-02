using System.ComponentModel.DataAnnotations;

namespace REST_API.Commands.EmployeeTerritory;
public class AddEmployeeTerritoryRequest
{
    [Required]
    public int EmployeeID { get; set; }
    [Required]
    [StringLength(20)]
    public string TerritoryID { get; set; }
}