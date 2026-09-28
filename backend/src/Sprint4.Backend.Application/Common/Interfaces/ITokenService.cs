using Sprint4.Backend.Domain.Enums;

namespace Sprint4.Backend.Application.Common.Interfaces
{
    /// <summary>
    /// Abstracción para la generación del token de acceso (US01).
    /// Interfaz pequeña y específica (Interface Segregation Principle): solo expone
    /// lo que un Handler necesita, nada de detalles de JWT ni de configuración.
    /// Si el equipo cambia de esquema de autenticación, se implementa esta misma
    /// interfaz en una clase nueva (Open/Closed Principle) y se registra en Program.cs.
    /// </summary>
    public interface ITokenService
    {
        string GenerateToken(int userId, string username, UserRole role);
    }
}
