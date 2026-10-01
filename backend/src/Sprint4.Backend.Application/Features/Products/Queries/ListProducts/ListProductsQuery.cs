using MediatR;
using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Application.Features.Products.Queries.ListProducts;

public sealed record ListProductsQuery : IRequest<IReadOnlyList<Product>>;
