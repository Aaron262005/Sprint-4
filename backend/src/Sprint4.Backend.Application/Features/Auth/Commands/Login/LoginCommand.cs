using MediatR;

namespace Sprint4.Backend.Application.Features.Auth.Commands.Login
{
    /// <summary>
    /// CQRS - Command: representa la INTENCIÓN de iniciar sesión.
    /// No contiene lógica, solo los datos necesarios para ejecutar la acción.
    /// Se envía a través de IMediator desde el AuthController.
    /// </summary>
    public record LoginCommand(string Username, string Password) : IRequest<LoginResult>;
}
