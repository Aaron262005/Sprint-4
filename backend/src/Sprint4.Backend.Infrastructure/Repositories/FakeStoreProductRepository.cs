using System.Net.Http.Json;
using System.Text.Json;
using Sprint4.Backend.Application.Common.Constants;
using Sprint4.Backend.Application.Common.Exceptions;
using Sprint4.Backend.Application.Common.Interfaces;
using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación concreta de IProductRepository que consume la Fake Store API (US03).
    /// Recibe un HttpClient ya configurado por inyección de dependencias (ver Program.cs),
    /// así esta clase no crea conexiones ni conoce la URL base: solo hace la petición.
    /// Si el equipo cambia de origen de datos, se crea otra clase que implemente
    /// IProductRepository y se cambia su registro en Program.cs (Open/Closed Principle).
    /// </summary>
    public class FakeStoreProductRepository : IProductRepository
    {
        private readonly HttpClient _httpClient;

        public FakeStoreProductRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                // El JSON de la API empata con la entidad Product (nota "Mapeo del JSON" de US03).
                var products = await _httpClient.GetFromJsonAsync<List<Product>>(
                    AppConstants.ExternalApis.FakeStore.ProductsPath,
                    cancellationToken);

                return products ?? new List<Product>();
            }
            catch (Exception ex) when (IsDataSourceFailure(ex) && !cancellationToken.IsCancellationRequested)
            {
                // Traduce el error técnico (red caída, timeout, JSON inválido) a una
                // excepción de Application, para que el Handler no dependa de HTTP.
                throw new DataSourceUnavailableException(AppConstants.ErrorMessages.ProductsUnavailable, ex);
            }
        }

        private static bool IsDataSourceFailure(Exception ex) =>
            ex is HttpRequestException or TaskCanceledException or JsonException or NotSupportedException;
    }
}
