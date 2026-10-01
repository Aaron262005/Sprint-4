namespace Sprint4.Backend.Domain.Entities
{
    /// <summary>
    /// Calificación de un producto. Empata con el objeto "rating" de la Fake Store API.
    /// </summary>
    public class ProductRating
    {
        public double Rate { get; set; }
        public int Count { get; set; }
    }
}
