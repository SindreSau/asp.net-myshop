using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop_Logging.Data;
using MyShop_Logging.DTO;

namespace MyShop_Logging.Controllers;

public class ProductController(AppDbContext context, ILogger<ProductController> logger, IMapper mapper) : Controller
{
    // GET: /products
    [HttpGet("/products")]
    public async Task<ActionResult<ProductReadDto>> GetProducts(int page = 1, int pageSize = 10)
    {
        try
        {
            var products = await context.Products
                .Include(p => p.Category)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(mapper.Map<IEnumerable<ProductReadDto>>(products));
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to get products");
            return StatusCode(500);
        }
    }
}