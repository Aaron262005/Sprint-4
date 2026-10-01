using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Application.Common.Interfaces
{
    /// <summary>
    /// Abstracción (Dependency Inversion Principle) para el acceso al catálogo de productos.
    /// La capa Application depende de ESTA interfaz, nunca de la Fake Store API directamente.
    /// Si el equipo cambia a una base de datos real o a un mock en memoria, se crea otra
    /// clase en Infrastructure que la implemente y se cambia su registro en Program.cs.
    ///
    /// TODO (equipo catálogo): US04 y US05 pueden agregar aquí sus propios métodos
    /// (ej. GetCategoriesAsync, GetByCategoryAsync, GetByIdAsync) sin modificar GetAllAsync.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// US03: devuelve el catálogo completo.
        /// Lanza <see cref="Exceptions.DataSourceUnavailableException"/> si el origen de datos falla.
        /// </summary>
        Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken);
    }
}
