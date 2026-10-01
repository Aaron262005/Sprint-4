using Sprint4.Backend.Application.Features.Products.DTOs;
using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Application.Common.Interfaces;

public interface IProductWriter
{
    Task<Product> CreateAsync(CreateProductDto product, CancellationToken cancellationToken);
    Task<Product> UpdateAsync(int id, UpdateProductDto product, CancellationToken cancellationToken);
    Task<Product> DeleteAsync(int id, CancellationToken cancellationToken);
}
