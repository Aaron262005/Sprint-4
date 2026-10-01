using MediatR;
using Sprint4.Backend.Domain.Entities;
using Sprint4.Backend.Application.Features.Products.DTOs;

namespace Sprint4.Backend.Application.Features.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(int Id, UpdateProductDto Product) : IRequest<Product>;
