using System.ComponentModel.DataAnnotations;

namespace REST_API.Commands.Territory;

public class EditTerritoryRequest
{
    [Required]
    [StringLength(20)]
    [RegularExpression("^[0-9]*$")]
    public string TerritoryID { get; set; }
    [Required]
    [StringLength(50)]
    public string TerritoryDescription { get; set; }
    [Required]
    public int RegionID { get; set; }
}
