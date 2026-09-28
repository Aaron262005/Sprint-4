using MediatR;

namespace Sprint4.Backend.Application.Features.Auth.Commands.Logout
{
    /// <summary>
    /// CQRS - Command de cierre de sesión (US02).
    /// Con JWT sin estado, el backend no "guarda" sesiones activas; el trabajo real
    /// (borrar token, resetear carrito, destruir historial de pantallas) ocurre en el
    /// cliente (ver AuthViewModel.logout() en el frontend). Este endpoint existe para
    /// permitir, a futuro, invalidar tokens (lista negra) sin romper el contrato con Angular.
    /// </summary>
    public record LogoutCommand(int UserId) : IRequest<bool>;
}
