using System.ComponentModel.DataAnnotations;

namespace REST_API.Commands.Category;

public class AddCategoryRequest
{
    [Required]
    public string CategoryName { get; set; }
    [MaxLength(15)]
    public string? Description { get; set; }
    public byte[]? Picture { get; set; } = null!;
}
