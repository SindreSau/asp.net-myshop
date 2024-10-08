using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MyShop_Logging.DTO;

public class ProductReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = String.Empty;
    public string ImageUrl { get; set; } = String.Empty;
    public string? CategoryName { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
}

public class ProductCreateDto
{
    [Required]
    [StringLength(100)]
    [MinLength(3)]
    public string Name { get; set; } = String.Empty;

    [Required]
    [Range(0.01, 1000000)]
    public decimal Price { get; set; }

    [Required]
    [StringLength(500)]
    [MinLength(3)]
    public string Description { get; set; } = String.Empty;

    [StringLength(1000)]
    public string ImageUrl { get; set; } = String.Empty;

    [Required]
    public int CategoryId { get; set; }
}

public class ProductUpdateDto
{
    [StringLength(100)]
    [MinLength(3)]
    public string Name { get; set; } = String.Empty;

    [Range(0.01, 1000000)]
    public decimal Price { get; set; }

    [StringLength(500)]
    [MinLength(3)]
    public string Description { get; set; } = String.Empty;

    [StringLength(1000)]
    public string ImageUrl { get; set; } = String.Empty;

    public int CategoryId { get; set; }
}
