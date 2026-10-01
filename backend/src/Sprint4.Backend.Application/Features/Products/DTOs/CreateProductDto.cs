using System.ComponentModel.DataAnnotations;
using Sprint4.Backend.Application.Common.Constants;
using Sprint4.Backend.Application.Features.Products.Validation;

namespace Sprint4.Backend.Application.Features.Products.DTOs;

public sealed record CreateProductDto
{
    [Required(ErrorMessage = AppConstants.Products.Required)]
    public string Title { get; init; } = string.Empty;

    [Range(typeof(decimal), AppConstants.Products.MinimumPrice, AppConstants.Products.MaximumPrice,
        ErrorMessage = AppConstants.Products.InvalidPrice, ParseLimitsInInvariantCulture = true, ConvertValueInInvariantCulture = true)]
    public decimal Price { get; init; }

    [Required(ErrorMessage = AppConstants.Products.Required)]
    public string Description { get; init; } = string.Empty;

    [Required(ErrorMessage = AppConstants.Products.Required), HttpsImage]
    public string Image { get; init; } = string.Empty;

    [Required(ErrorMessage = AppConstants.Products.Required)]
    public string Category { get; init; } = string.Empty;
}
