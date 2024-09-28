using System.ComponentModel.DataAnnotations;

namespace REST_API.Commands.Category;

public class EditCategoryRequest
{
    public int CategoryID { get; set; }
    [MaxLength(15)]
    public string CategoryName { get; set; }
    public string? Description { get; set; }
    public byte[]? Picture { get; set; }
}
