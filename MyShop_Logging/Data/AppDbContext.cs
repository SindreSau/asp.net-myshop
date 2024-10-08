using Microsoft.EntityFrameworkCore;
using MyShop_Logging.Models;

namespace MyShop_Logging.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    // DbSet for each entity:
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
}