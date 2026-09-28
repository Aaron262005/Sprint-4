using Sprint4.Backend.Application.Common.Interfaces;
using Sprint4.Backend.Domain.Entities;
using Sprint4.Backend.Infrastructure.Persistence;

namespace Sprint4.Backend.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación concreta de IUserRepository sobre el mock en memoria.
    /// Cuando se conecte EF Core o la Fake Store API, se crea OTRA clase que
    /// implemente IUserRepository y se cambia el registro en Program.cs — un solo
    /// lugar de cambio, nada en Application ni en API se toca.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        public User? GetByCredentials(string username, string password)
        {
            return MockUserDataStore.Users.FirstOrDefault(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);
        }

        public User? GetById(int id)
        {
            return MockUserDataStore.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}
