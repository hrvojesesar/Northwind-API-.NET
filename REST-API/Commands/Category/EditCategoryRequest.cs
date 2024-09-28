namespace REST_API.Commands.Category;

public class EditCategoryRequest
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; }
    public string? Description { get; set; }
    public byte[]? Picture { get; set; }
}
