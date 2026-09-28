using MediatR;
using Sprint4.Backend.Application.Common.Constants;
using Sprint4.Backend.Application.Common.Interfaces;

namespace Sprint4.Backend.Application.Features.Auth.Commands.Login
{
    /// <summary>
    /// Orquesta el caso de uso "Login" (US01, escenarios 1 y 2).
    /// Depende ÚNICAMENTE de abstracciones (IUserRepository, IRoleMapper, ITokenService):
    /// Dependency Inversion Principle en acción. No sabe si los usuarios vienen de una
    /// lista en memoria, de SQL Server o de la Fake Store API — eso no es asunto suyo.
    /// </summary>
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleMapper _roleMapper;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IRoleMapper roleMapper,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _roleMapper = roleMapper;
            _tokenService = tokenService;
        }

        public Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetByCredentials(request.Username, request.Password);

            if (user is null)
            {
                // US01 - Escenario 2: credenciales incorrectas -> el Controller devolverá 401.
                return Task.FromResult(LoginResult.Fail(AppConstants.ErrorMessages.InvalidCredentials));
            }

            var role = _roleMapper.MapRole(user.Id);
            var token = _tokenService.GenerateToken(user.Id, user.Username, role);

            // US01 - Escenario 1: autenticación exitosa y mapeo de rol -> el Controller devolverá 200 OK.
            return Task.FromResult(LoginResult.Ok(token, user.Id, user.Username, role.ToString()));
        }
    }
}
