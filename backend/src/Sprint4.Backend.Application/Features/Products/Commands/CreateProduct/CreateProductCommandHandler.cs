using System.ComponentModel.DataAnnotations;
using MediatR;
using Sprint4.Backend.Application.Common.Constants;
using Sprint4.Backend.Application.Common.Interfaces;
using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Application.Features.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler(IProductWriter writer)
    : IRequestHandler<CreateProductCommand, Product>
{
    public Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Validator.ValidateObject(request.Product, new ValidationContext(request.Product), true);
        return writer.CreateAsync(request.Product, cancellationToken);
    }
}
