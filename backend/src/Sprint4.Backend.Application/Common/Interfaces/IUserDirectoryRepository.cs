using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Application.Common.Interfaces
{
    public interface IUserDirectoryRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}