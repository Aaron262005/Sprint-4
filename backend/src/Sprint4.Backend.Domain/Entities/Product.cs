namespace Sprint4.Backend.Domain.Entities
{
    /// <summary>
    /// Representa un artículo del catálogo (US03).
    /// Sus propiedades empatan exactamente con la estructura JSON de la Fake Store API
    /// (id, title, price, description, category, image, rating), tal como pide la
    /// nota "Mapeo del JSON" de la historia. Al vivir en Domain, no depende de HTTP,
    /// de EF Core ni de ningún detalle técnico (Dependency Inversion Principle).
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        /// <summary>URL (texto) de la imagen. El frontend la descarga de forma asíncrona.</summary>
        public string Image { get; set; } = string.Empty;

        public ProductRating Rating { get; set; } = new();
    }
}
