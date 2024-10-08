using MyShop_Logging.Models;

namespace MyShop_Logging.Data;

public class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Products.Any())
        {
            return;
        }

        var categories = new[]
        {
            new Category { Name = "Electronics", Description = "Electronic products" },
            new Category { Name = "Clothing", Description = "Clothing products" },
            new Category { Name = "Books", Description = "Books" }
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();

        var products = new[]
        {
            new Product
            {
                Name = "Laptop",
                Price = 1000,
                Description = "A laptop",
                ImageUrl = "https://example.com/laptop.jpg",
                CategoryId = categories[0].Id
            },
            new Product
            {
                Name = "T-shirt",
                Price = 20,
                Description = "A t-shirt",
                ImageUrl = "https://example.com/t-shirt.jpg",
                CategoryId = categories[1].Id
            },
            new Product
            {
                Name = "Book",
                Price = 10,
                Description = "A book",
                ImageUrl = "https://example.com/book.jpg",
                CategoryId = categories[2].Id
            }
        };

        context.Products.AddRange(products);
        context.SaveChanges();
    }
}