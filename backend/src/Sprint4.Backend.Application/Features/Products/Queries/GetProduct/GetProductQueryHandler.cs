using System.ComponentModel.DataAnnotations;
using MediatR;
using Sprint4.Backend.Application.Common.Constants;
using Sprint4.Backend.Application.Common.Interfaces;
using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Application.Features.Products.Queries.GetProduct;

public sealed class GetProductQueryHandler(IProductReader reader) : IRequestHandler<GetProductQuery, Product?>
{
    public Task<Product?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        if (request.Id <= 0)
            throw new ValidationException(AppConstants.Products.InvalidId);
        return reader.GetAsync(request.Id, cancellationToken);
    }
}
