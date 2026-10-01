using Sprint4.Backend.Application.Common.Constants;

namespace Sprint4.Backend.Application.Features.Products;

public sealed class ProductNotFoundException() : Exception(AppConstants.Products.NotFound);
