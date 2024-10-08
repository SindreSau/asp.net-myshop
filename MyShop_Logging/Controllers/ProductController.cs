using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyShop_Logging.DTO;
using MyShop_Logging.Repositories.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace MyShop_Logging.Controllers;

public class ProductController(IProductRepository productRepository, ILogger<ProductController> logger, IMapper mapper)
    : Controller
{
    // GET: /products
    [HttpGet("/products")]
    [SwaggerOperation(Summary = "Get all products with pagination and optional filtering")]
    [SwaggerResponse(StatusCodes.Status200OK, "The products were found", typeof(IEnumerable<ProductReadDto>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Failed to get the products")]
    public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetProducts(
        int page = 1,
        int pageSize = 10,
        string? searchQuery = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        int? categoryId = null
        )
    {
        try
        {
            var (products, totalCount) = await productRepository.GetProducts(page, pageSize, searchQuery, minPrice, maxPrice, categoryId);

            Response.Headers.Append("X-Pagination", System.Text.Json.JsonSerializer.Serialize(new
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page,
            }));

            return Ok(mapper.Map<IEnumerable<ProductReadDto>>(products));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get products");
            return StatusCode(500);
        }
    }

    // GET: /products/{id}
    [HttpGet("/products/{id}")]
    [SwaggerOperation(Summary = "Get a product by ID")]
    [SwaggerResponse(StatusCodes.Status200OK, "The product was found", typeof(ProductReadDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The product was not found")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Failed to get the product")]
    public async Task<ActionResult<ProductReadDto>> GetProduct(int id)
    {
        try
        {
            var product = await productRepository.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ProductReadDto>(product));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get product");
            return StatusCode(500);
        }
    }

    // POST: /products
    [HttpPost("/products")]
    [SwaggerOperation(Summary = "Create a new product")]
    [SwaggerResponse(StatusCodes.Status201Created, "The product was created", typeof(ProductReadDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Failed to create the product")]
    public async Task<ActionResult<ProductReadDto>> CreateProduct([FromBody]ProductCreateDto productCreateDto)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var product = await productRepository.CreateProduct(productCreateDto);

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, mapper.Map<ProductReadDto>(product));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get product with name {0}", productCreateDto.Name);
            return StatusCode(500);
        }
    }

    // PUT: /products/{id}
    [HttpPut("/products/{id}")]
    [SwaggerOperation(Summary = "Update a product")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The product was updated")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The product was not found")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Failed to update the product")]
    public async Task<ActionResult> UpdateProduct(int id, [FromBody]ProductUpdateDto productUpdateDto)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var product = await productRepository.UpdateProduct(id, productUpdateDto);

            if (product == null)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to update product with ID {0}", id);
            return StatusCode(500);
        }
    }

    // DELETE: /products/{id}
    [HttpDelete("/products/{id}")]
    [SwaggerOperation(Summary = "Delete a product")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The product was deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The product was not found")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Failed to delete the product")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var result = await productRepository.DeleteProduct(id);

        if (result == false)
        {
            return NotFound();
        }

        return NoContent();
    }
}