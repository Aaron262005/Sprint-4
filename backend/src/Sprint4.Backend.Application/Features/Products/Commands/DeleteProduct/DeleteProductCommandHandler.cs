using System.ComponentModel.DataAnnotations;
using MediatR;
using Sprint4.Backend.Application.Common.Constants;
using Sprint4.Backend.Application.Common.Interfaces;
using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Application.Features.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommandHandler(IProductWriter writer)
    : IRequestHandler<DeleteProductCommand, Product>
{
    public Task<Product> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        if (request.Id <= 0)
            throw new ValidationException(AppConstants.Products.InvalidId);
        return writer.DeleteAsync(request.Id, cancellationToken);
    }
}
