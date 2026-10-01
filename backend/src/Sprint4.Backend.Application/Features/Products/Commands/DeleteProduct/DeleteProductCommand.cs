using MediatR;
using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Application.Features.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand(int Id) : IRequest<Product>;
