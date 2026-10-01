using MediatR;
using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Application.Features.Products.Queries.GetProduct;

public sealed record GetProductQuery(int Id) : IRequest<Product?>;
