using Sprint4.Backend.Application.Common.Interfaces;
using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Infrastructure.Persistence
{
    public class MockUserDirectoryRepository : IUserDirectoryRepository
    {
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = new List<User>
            {
                new User 
                { 
                    Id = 1, Name = "Admin System", Email = "admin@empresa.com", Phone = "4271234567", Username = "admin1", Role = "Administrador",
                    Address = new Address { Street = "Av. Central", City = "San Juan del Río", Geolocation = new Geolocation { Lat = "20.38", Long = "-100.00" } }
                },
                new User 
                { 
                    Id = 2, Name = "Cliente Frecuente", Email = "cliente@correo.com", Phone = "4279876543", Username = "clientex", Role = "Cliente",
                    Address = new Address { Street = "Calle 2", City = "Querétaro", Geolocation = new Geolocation { Lat = "20.58", Long = "-100.38" } }
                }
            };
            
            return await Task.FromResult(users);
        }
    }
}