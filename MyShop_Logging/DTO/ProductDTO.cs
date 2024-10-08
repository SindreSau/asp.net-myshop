namespace MyShop_Logging.DTO;

public record ProductReadDto(
    int Id,
    string Name,
    decimal Price,
    string Description,
    string ImageUrl,
    string? CategoryName
);

public record ProductCreateDto(
    string Name,
    decimal Price,
    string Description,
    string ImageUrl,
    int CategoryId
);

public record ProductUpdateDto(
    int Id,
    string Name,
    decimal Price,
    string Description,
    string ImageUrl,
    int CategoryId
);