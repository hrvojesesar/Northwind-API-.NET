using System.ComponentModel.DataAnnotations;

namespace REST_API.Commands.CustomerCustomerDemo;

public class AddCustomerCustomerDemoRequest
{
    [Required]
    [MaxLength(5)]
    [RegularExpression("^[A-Z]*$")]
    public string CustomerID { get; set; }
    [Required]
    [MaxLength(10)]
    [RegularExpression("^[0-9]*$")]
    public string CustomerTypeID { get; set; }
}
