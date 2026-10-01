using MediatR;

namespace Sprint4.Backend.Application.Features.Products.Queries.GetAllProducts
{
    /// <summary>
    /// CQRS - Query de US03: representa la INTENCIÓN de leer el catálogo completo.
    /// Es una Query (lectura) y no un Command porque no modifica ningún dato.
    /// Se envía a través de IMediator desde el ProductsController.
    /// </summary>
    public record GetAllProductsQuery() : IRequest<GetAllProductsResult>;
}
