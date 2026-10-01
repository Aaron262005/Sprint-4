using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Application.Common.Interfaces;

public interface IProductReader
{
    Task<IReadOnlyList<Product>> ListAsync(CancellationToken cancellationToken);
    Task<Product?> GetAsync(int id, CancellationToken cancellationToken);
}
