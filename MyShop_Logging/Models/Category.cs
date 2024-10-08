namespace MyShop_Logging.Models;

public class Category : BaseEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}