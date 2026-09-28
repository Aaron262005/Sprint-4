using Sprint4.Backend.Domain.Enums;

namespace Sprint4.Backend.Application.Common.Interfaces
{
    /// <summary>
    /// Encapsula la regla de negocio de US01 "¿qué rol le corresponde a este Id de usuario?".
    /// Separarla en su propia interfaz respeta el Single Responsibility Principle:
    /// el LoginCommandHandler no necesita saber CÓMO se calcula el rol, solo que existe.
    /// </summary>
    public interface IRoleMapper
    {
        UserRole MapRole(int userId);
    }
}
