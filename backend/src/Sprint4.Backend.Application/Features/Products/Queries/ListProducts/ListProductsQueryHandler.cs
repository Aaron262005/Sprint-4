using MediatR;
using Sprint4.Backend.Application.Common.Interfaces;
using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Application.Features.Products.Queries.ListProducts;

public sealed class ListProductsQueryHandler(IProductReader reader)
    : IRequestHandler<ListProductsQuery, IReadOnlyList<Product>>
{
    public Task<IReadOnlyList<Product>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
        => reader.ListAsync(cancellationToken);
}
