using Sprint4.Backend.Domain.Entities;

namespace Sprint4.Backend.Application.Common.Interfaces
{
    /// <summary>
    /// Abstracción (Dependency Inversion Principle) para el acceso a los datos de usuarios.
    /// La capa Application depende de ESTA interfaz, nunca de una implementación concreta.
    /// Cuando el equipo conecte una base de datos real o la Fake Store API, se crea una
    /// nueva clase en Infrastructure que la implemente; los Handlers no cambian ni una línea.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>Busca un usuario por username/password. Devuelve null si no existe (US01 escenario 2).</summary>
        User? GetByCredentials(string username, string password);

        User? GetById(int id);
    }
}
