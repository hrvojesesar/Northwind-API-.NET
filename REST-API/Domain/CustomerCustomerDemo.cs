using System.ComponentModel.DataAnnotations;

namespace REST_API.Domain;

public class CustomerCustomerDemo
{
    [Key]
    public string CustomerID { get; set; }
    public string CustomerTypeID { get; set; }
}
