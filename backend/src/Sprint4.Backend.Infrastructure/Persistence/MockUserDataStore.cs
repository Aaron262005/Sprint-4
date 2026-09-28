using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Infrastructure.Persistence
{
    /// <summary>
    /// Simula la tabla/colección de usuarios (Administradores, Cliente y Auditor)
    /// mientras el equipo no conecta una base de datos real o la Fake Store API.
    /// Al estar aislada en una sola clase, el día de mañana solo se reemplaza ESTA
    /// clase por el nuevo origen de datos (Open/Closed Principle) — UserRepository,
    /// los Handlers y los Controllers no se enteran del cambio.
    /// </summary>
    public static class MockUserDataStore
    {
        public static readonly List<User> Users = new()
        {
            // IDs 1 y 2 -> Administrador (regla de negocio de US01)
            new User { Id = 1, Username = "admin1",   Password = "Admin123!",   FullName = "Aarón Pérez",    Email = "admin1@sprint4.com" },
            new User { Id = 2, Username = "admin2",   Password = "Admin456!",   FullName = "Laura Gómez",    Email = "admin2@sprint4.com" },

            // ID 3 -> Auditor
            new User { Id = 3, Username = "auditor1", Password = "Auditor123!", FullName = "Carlos Auditor", Email = "auditor1@sprint4.com" },

            // Resto de IDs -> Cliente
            new User { Id = 4, Username = "cliente1", Password = "Cliente123!", FullName = "Erick Rafael",   Email = "cliente1@sprint4.com" },
            new User { Id = 5, Username = "cliente2", Password = "Cliente456!", FullName = "Mariana López",  Email = "cliente2@sprint4.com" },
        };
    }
}
