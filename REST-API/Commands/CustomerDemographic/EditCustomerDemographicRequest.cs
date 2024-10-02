using System.ComponentModel.DataAnnotations;

namespace REST_API.Commands.CustomerDemographic;
public class EditCustomerDemographicRequest
{
    [Required]
    [RegularExpression("^[0-9]*$")]
    [StringLength(10, ErrorMessage = "CustomerTypeID can't be longer than 10 characters.")]
    public string CustomerTypeID { get; set; }
    public string? CustomerDesc { get; set; }
}