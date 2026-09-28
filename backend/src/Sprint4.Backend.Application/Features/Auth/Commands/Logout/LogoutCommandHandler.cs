using MediatR;

namespace Sprint4.Backend.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, bool>
    {
        // TODO (equipo): si más adelante se agrega una lista negra de tokens,
        // inyectar aquí una nueva abstracción (ej. ITokenBlacklistService) siguiendo
        // el mismo patrón que IUserRepository / ITokenService.
        public Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}
