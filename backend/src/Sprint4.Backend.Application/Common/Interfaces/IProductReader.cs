using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Application.Common.Interfaces;

public interface IProductReader
{
    Task<Product?> GetAsync(int id, CancellationToken cancellationToken);
}
