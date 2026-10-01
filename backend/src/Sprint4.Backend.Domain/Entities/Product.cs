namespace Sprint4.Backend.Domain.Entities;

public sealed record Product(int Id, string Title, decimal Price, string Description,
    string Image, string Category);
