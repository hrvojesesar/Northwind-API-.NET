using System.ComponentModel.DataAnnotations;

namespace REST_API.Domain;

public class CustomerDemographic
{
    [Key]
    public string CustomerTypeID { get; set; }
    public string? CustomerDesc { get; set; }
}
