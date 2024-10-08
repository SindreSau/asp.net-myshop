using MyShop_Logging.DTO;
using MyShop_Logging.Models;

namespace MyShop_Logging.Repositories.Interfaces;

public interface IProductRepository
{
    Task<(IEnumerable<Product> Products, int TotalCount)> GetProducts(int page, int pageSize, string? searchQuery, decimal? minPrice, decimal? maxPrice, int? categoryId);
    Task<Product?> GetProduct(int id);
    Task<Product> CreateProduct(ProductCreateDto productCreateDto);
    Task<Product?> UpdateProduct(int id, ProductUpdateDto productUpdateDto);
    Task<bool> DeleteProduct(int id);
}