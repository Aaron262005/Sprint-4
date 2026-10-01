using System.Text.Json;
using Sprint4.Backend.Application.Common.Constants;
using Sprint4.Backend.Application.Common.Interfaces;
using Sprint4.Backend.Application.Features.Products;
using Sprint4.Backend.Application.Features.Products.DTOs;
using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Infrastructure.Persistence;

// Almacén sencillo para UNA instancia de la API. No consume servicios externos.
// El semáforo permite atender una escritura a la vez, sin perder productos.
public sealed class JsonProductRepository(string filePath) : IProductReader, IProductWriter, IDisposable
{
    private readonly SemaphoreSlim gate = new(1, 1);

    public async Task<IReadOnlyList<Product>> ListAsync(CancellationToken cancellationToken)
    {
        await gate.WaitAsync(cancellationToken);
        try { return (await ReadAsync(cancellationToken)).Products.ToArray(); }
        finally { gate.Release(); }
    }

    public async Task<Product?> GetAsync(int id, CancellationToken cancellationToken)
    {
        var products = await ListAsync(cancellationToken);
        return products.FirstOrDefault(product => product.Id == id);
    }

    public async Task<Product> CreateAsync(CreateProductDto product, CancellationToken cancellationToken)
    {
        await gate.WaitAsync(cancellationToken);
        try
        {
            var data = await ReadAsync(cancellationToken);
            var created = new Product(data.NextId, product.Title.Trim(), product.Price,
                product.Description.Trim(), product.Image.Trim(), product.Category.Trim());
            data.NextId = checked(data.NextId + 1);
            data.Products.Add(created);
            await SaveAsync(data, cancellationToken);
            return created;
        }
        finally { gate.Release(); }
    }

    public async Task<Product> UpdateAsync(int id, UpdateProductDto product, CancellationToken cancellationToken)
    {
        await gate.WaitAsync(cancellationToken);
        try
        {
            var data = await ReadAsync(cancellationToken);
            var index = data.Products.FindIndex(item => item.Id == id);
            if (index < 0) throw new ProductNotFoundException();
            var updated = new Product(id, product.Title.Trim(), product.Price,
                product.Description.Trim(), product.Image.Trim(), product.Category.Trim());
            data.Products[index] = updated;
            await SaveAsync(data, cancellationToken);
            return updated;
        }
        finally { gate.Release(); }
    }

    public async Task<Product> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        await gate.WaitAsync(cancellationToken);
        try
        {
            var data = await ReadAsync(cancellationToken);
            var product = data.Products.FirstOrDefault(item => item.Id == id);
            if (product is null) throw new ProductNotFoundException();
            data.Products.Remove(product);
            await SaveAsync(data, cancellationToken);
            return product;
        }
        finally { gate.Release(); }
    }

    private async Task<ProductFile> ReadAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(filePath)) return new ProductFile();
        await using var stream = File.OpenRead(filePath);
        var data = await JsonSerializer.DeserializeAsync<ProductFile>(stream, cancellationToken: cancellationToken);
        // Nunca reemplazar silenciosamente un archivo inválido por una lista vacía.
        if (data is null || data.Products is null || data.NextId <= 0
            || data.Products.Any(product => product is null || product.Id <= 0 || product.Id >= data.NextId)
            || data.Products.Select(product => product.Id).Distinct().Count() != data.Products.Count)
            throw new JsonException(AppConstants.Products.StorageError);
        return data;
    }

    private async Task SaveAsync(ProductFile data, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(filePath))!);
        var temporary = filePath + AppConstants.Products.TemporaryExtension;
        try
        {
            await using (var stream = File.Create(temporary))
            {
                await JsonSerializer.SerializeAsync(stream, data, cancellationToken: cancellationToken);
                await stream.FlushAsync(cancellationToken);
            }
            // Solo sustituimos el archivo original cuando el nuevo está completo.
            File.Move(temporary, filePath, overwrite: true);
        }
        finally
        {
            if (File.Exists(temporary)) File.Delete(temporary);
        }
    }

    public void Dispose() => gate.Dispose();

    public sealed class ProductFile
    {
        public int NextId { get; set; } = 1;
        public List<Product> Products { get; set; } = [];
    }
}
