using System.ComponentModel.DataAnnotations;

namespace REST_API.Commands.OrderDetail;

public class AddOrderDetailRequest
{
    [Required]
    public int OrderID { get; set; }
    [Required]
    public int ProductID { get; set; }
    [Required]
    public decimal UnitPrice { get; set; }
    [Required]
    public short Quantity { get; set; }
    [Required]
    public float Discount { get; set; }
}
