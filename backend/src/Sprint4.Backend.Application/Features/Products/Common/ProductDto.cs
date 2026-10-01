using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Application.Features.Products.Common
{
    /// <summary>
    /// DTO de salida de un producto. Desacopla lo que recibe el frontend de la entidad
    /// de dominio "Product". Vive en Features/Products/Common para que US04 (filtro por
    /// categoría) y US05 (detalle) lo reutilicen sin duplicarlo.
    /// </summary>
    public record ProductDto(
        int Id,
        string Title,
        decimal Price,
        string Description,
        string Category,
        string Image,
        ProductRatingDto Rating)
    {
        /// <summary>Única responsabilidad: traducir la entidad de dominio a su DTO.</summary>
        public static ProductDto FromEntity(Product product)
        {
            // Guard clause: si la API no envió "rating", se devuelve una calificación vacía
            // en lugar de null, para que el frontend no tenga que validar nulos.
            var rating = product.Rating is null
                ? new ProductRatingDto(0, 0)
                : new ProductRatingDto(product.Rating.Rate, product.Rating.Count);

            return new ProductDto(
                product.Id,
                product.Title,
                product.Price,
                product.Description,
                product.Category,
                product.Image,
                rating);
        }
    }

    public record ProductRatingDto(double Rate, int Count);
}
