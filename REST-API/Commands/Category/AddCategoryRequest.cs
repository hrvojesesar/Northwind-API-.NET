using System.ComponentModel.DataAnnotations;

namespace REST_API.Commands.Category;
public class AddCategoryRequest
{
    public string CategoryName { get; set; }
    public string? Description { get; set; }
    public byte[]? Picture { get; set; } = null!;
}