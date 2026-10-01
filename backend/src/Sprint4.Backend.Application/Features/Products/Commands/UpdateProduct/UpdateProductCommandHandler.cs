using System.ComponentModel.DataAnnotations;
using MediatR;
using Sprint4.Backend.Application.Common.Constants;
using Sprint4.Backend.Application.Common.Interfaces;
using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Application.Features.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler(IProductWriter writer)
    : IRequestHandler<UpdateProductCommand, Product>
{
    public Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        if (request.Id <= 0)
            throw new ValidationException(AppConstants.Products.InvalidId);
        Validator.ValidateObject(request.Product, new ValidationContext(request.Product), true);
        return writer.UpdateAsync(request.Id, request.Product, cancellationToken);
    }
}
