using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyShop_Logging.Data;
using MyShop_Logging.DTO;
using MyShop_Logging.Models;
using MyShop_Logging.Repositories.Interfaces;

namespace MyShop_Logging.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ProductRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<(IEnumerable<Product> Products, int TotalCount)> GetProducts(
        int page,
        int pageSize,
        string? searchQuery,
        decimal? minPrice,
        decimal? maxPrice,
        int? categoryId
    )
    {
        var queryable = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            queryable = queryable.Where(
                p =>
                    p.Name != null && p.Description != null && p.Category != null && p.Category.Name != null && (
                        p.Name.Contains(searchQuery) || p.Description.Contains(searchQuery) ||
                        p.Category.Name.Contains(searchQuery)
                    )
            );
        }

        if (minPrice.HasValue)
        {
            queryable = queryable.Where(p => p.Price >= minPrice);
        }

        if (maxPrice.HasValue)
        {
            queryable = queryable.Where(p => p.Price <= maxPrice);
        }

        if (categoryId.HasValue)
        {
            queryable = queryable.Where(p => p.CategoryId == categoryId);
        }

        var totalCount = await queryable.CountAsync();

        var products = await queryable
            .Include(p => p.Category)
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (products, totalCount);
    }

    public async Task<Product?> GetProduct(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        return product;
    }

    public async Task<Product> CreateProduct(ProductCreateDto productCreateDto)
    {
        var product = _mapper.Map<Product>(productCreateDto);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<Product?> UpdateProduct(int id, ProductUpdateDto productUpdateDto)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null) return null;

        _mapper.Map(productUpdateDto, product);

        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<bool> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }
}