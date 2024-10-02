using System.ComponentModel.DataAnnotations;

namespace REST_API.Commands.Shipper;
public class EditShipperRequest
{
    public int ShipperID { get; set; }
    [Required]
    [MaxLength(40)]
    public string CompanyName { get; set; }
    [MaxLength(24)]
    public string? Phone { get; set; }
}