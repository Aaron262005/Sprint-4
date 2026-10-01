using MediatR;
using Sprint4.Backend.Application.Common.Exceptions;
using Sprint4.Backend.Application.Common.Interfaces;
using Sprint4.Backend.Application.Features.Products.Common;

namespace Sprint4.Backend.Application.Features.Products.Queries.GetAllProducts
{
    /// <summary>
    /// Orquesta el caso de uso "Visualizar catálogo general" (US03).
    /// Depende ÚNICAMENTE de la abstracción IProductRepository (Dependency Inversion Principle):
    /// no sabe si los productos vienen de la Fake Store API, de un mock o de SQL Server.
    /// </summary>
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, GetAllProductsResult>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<GetAllProductsResult> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var products = await _productRepository.GetAllAsync(cancellationToken);

                // US03 - Escenario 1: catálogo obtenido correctamente.
                var dtos = products.Select(ProductDto.FromEntity).ToList();
                return GetAllProductsResult.Ok(dtos);
            }
            catch (DataSourceUnavailableException ex)
            {
                // US03 - Escenario 3: el origen de datos falló -> el Controller devolverá 503.
                return GetAllProductsResult.Fail(ex.Message);
            }
        }
    }
}
