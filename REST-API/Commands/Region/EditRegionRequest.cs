using System.ComponentModel.DataAnnotations;

namespace REST_API.Commands.Region;
public class EditRegionRequest
{
    [Required]
    public int RegionID { get; set; }
    [Required]
    [StringLength(50)]
    public string RegionDescription { get; set; }
}