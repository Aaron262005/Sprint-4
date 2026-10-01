using Sprint4.Backend.Application.Features.Products.Common;

namespace Sprint4.Backend.Application.Features.Products.Queries.GetAllProducts
{
    /// <summary>
    /// Resultado de la Query de US03. Sigue el mismo patrón que LoginResult (Ok / Fail),
    /// para que el Controller decida la respuesta HTTP sin conocer la causa técnica del error.
    /// </summary>
    public class GetAllProductsResult
    {
        public bool Success { get; private init; }
        public IReadOnlyList<ProductDto> Products { get; private init; } = Array.Empty<ProductDto>();
        public string? ErrorMessage { get; private init; }

        public static GetAllProductsResult Ok(IReadOnlyList<ProductDto> products) => new()
        {
            Success = true,
            Products = products
        };

        public static GetAllProductsResult Fail(string message) => new()
        {
            Success = false,
            ErrorMessage = message
        };
    }
}
