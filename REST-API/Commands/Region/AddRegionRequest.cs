using System.ComponentModel.DataAnnotations;

namespace REST_API.Commands.Region;
public class AddRegionRequest
{
    [Required]
    public int RegionID { get; set; }
    [Required]
    [StringLength(50)]
    public string RegionDescription { get; set; }
}