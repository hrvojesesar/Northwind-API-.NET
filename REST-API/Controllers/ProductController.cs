using Microsoft.AspNetCore.Mvc;
using REST_API.Commands.Product;
using REST_API.Domain;
using REST_API.Interfaces;

namespace REST_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("GetAllProducts")]
    [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productRepository.GetAllProductsAsync();
        if (products == null)
        {
            return NotFound();
        }
        return Ok(products);
    }

    /// <summary>
    /// Get product by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("GetProductById/{id}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductById(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var product = await _productRepository.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    /// <summary>
    /// Add new product
    /// </summary>
    /// <param name="addProductRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("AddProduct")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddProduct(AddProductRequest addProductRequest)
    {
        if (addProductRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = new Product
        {
            ProductName = addProductRequest.ProductName,
            SupplierID = addProductRequest.SupplierID,
            CategoryID = addProductRequest.CategoryID,
            QuantityPerUnit = addProductRequest.QuantityPerUnit,
            UnitPrice = addProductRequest.UnitPrice,
            UnitsInStock = addProductRequest.UnitsInStock,
            UnitsOnOrder = addProductRequest.UnitsOnOrder,
            ReorderLevel = addProductRequest.ReorderLevel,
            Discontinued = addProductRequest.Discontinued
        };

        var addedProduct = await _productRepository.AddProductAsync(product);
        return CreatedAtAction(nameof(GetProductById), new { id = addedProduct.ProductID }, addedProduct);
    }

    /// <summary>
    /// Edit product
    /// </summary>
    /// <param name="editProductRequest"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("EditProduct")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditProduct(EditProductRequest editProductRequest)
    {
        if (editProductRequest == null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = new Product
        {
            ProductID = editProductRequest.ProductID,
            ProductName = editProductRequest.ProductName,
            SupplierID = editProductRequest.SupplierID,
            CategoryID = editProductRequest.CategoryID,
            QuantityPerUnit = editProductRequest.QuantityPerUnit,
            UnitPrice = editProductRequest.UnitPrice,
            UnitsInStock = editProductRequest.UnitsInStock,
            UnitsOnOrder = editProductRequest.UnitsOnOrder,
            ReorderLevel = editProductRequest.ReorderLevel,
            Discontinued = editProductRequest.Discontinued
        };

        var editedProduct = await _productRepository.EditProductAsync(product);
        if (editedProduct == null)
        {
            return NotFound();
        }
        return Ok(editedProduct);
    }

    /// <summary>
    /// Delete product
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("DeleteProduct/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteProduct(int? id)
    {
        if (id == null)
        {
            return BadRequest();
        }

        var result = await _productRepository.DeleteProductAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return Ok($"Product with id: {id} is successfully deleted!");
    }
}
