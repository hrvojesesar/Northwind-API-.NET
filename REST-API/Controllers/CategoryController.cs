using Microsoft.AspNetCore.Mvc;
using REST_API.Commands.Category;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    /// <summary>
    /// Get all categories
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("GetAllCategories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAllCategoriesAsync();
        return Ok(categories);
    }

    /// <summary>
    /// Get category by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetCategoryById/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCategoryById(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var category = await _categoryRepository.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(category);
    }

    /// <summary>
    /// Add new category
    /// </summary>
    /// <param name="addCategoryRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("AddCategory")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddCategory(AddCategoryRequest addCategoryRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var category = new Category
        {
            CategoryName = addCategoryRequest.CategoryName,
            Description = addCategoryRequest.Description,
            Picture = addCategoryRequest.Picture
        };

        var addedCategory = await _categoryRepository.AddCategoryAsync(category);
        return CreatedAtAction(nameof(GetCategoryById), new { id = addedCategory.CategoryID }, addedCategory);
    }

    /// <summary>
    /// Edit existing category
    /// </summary>
    /// <param name="editCategoryRequest"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("EditCategory")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditCategory(EditCategoryRequest editCategoryRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var category = new Category
        {
            CategoryID = editCategoryRequest.CategoryID,
            CategoryName = editCategoryRequest.CategoryName,
            Description = editCategoryRequest.Description,
            Picture = editCategoryRequest.Picture
        };

        var editedCategory = await _categoryRepository.EditCategoryAsync(category);

        if (editedCategory == null)
        {
            return NotFound();
        }

        return Ok(editedCategory);
    }

    /// <summary>
    /// Delete category by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("DeleteCategory/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteCategory(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var isDeleted = await _categoryRepository.DeleteCategoryAsync(id);

        if (!isDeleted)
        {
            return NotFound();
        }

        return Ok($"Category with id: {id} is successfully deleted!");
    }
}
