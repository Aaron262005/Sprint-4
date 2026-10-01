using System.Net;
using System.Net.Http.Json;
using Sprint4.Backend.Application.Common.Constants;
using Sprint4.Backend.Application.Common.Interfaces;
using Sprint4.Backend.Application.Features.Products.DTOs;
using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Infrastructure.Services;

// Fake Store confirma las escrituras, pero no las guarda en su base de datos.
public sealed class FakeStoreApiService(HttpClient http) : IProductReader, IProductWriter
{
    public async Task<Product?> GetAsync(int id, CancellationToken cancellationToken)
    {
        using var response = await http.GetAsync(ItemPath(id), cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Product>(cancellationToken);
    }

    public async Task<Product> CreateAsync(CreateProductDto product, CancellationToken cancellationToken)
    {
        using var response = await http.PostAsJsonAsync(AppConstants.Products.Route, product, cancellationToken);
        return await ReadProductAsync(response, cancellationToken);
    }

    public async Task<Product> UpdateAsync(int id, UpdateProductDto product, CancellationToken cancellationToken)
    {
        using var response = await http.PutAsJsonAsync(ItemPath(id), product, cancellationToken);
        return await ReadProductAsync(response, cancellationToken);
    }

    public async Task<Product> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        using var response = await http.DeleteAsync(ItemPath(id), cancellationToken);
        return await ReadProductAsync(response, cancellationToken);
    }

    private static string ItemPath(int id) => $"{AppConstants.Products.Route}/{id}";

    private static async Task<Product> ReadProductAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Product>(cancellationToken)
            ?? throw new HttpRequestException(AppConstants.Products.ProviderError);
    }
}
