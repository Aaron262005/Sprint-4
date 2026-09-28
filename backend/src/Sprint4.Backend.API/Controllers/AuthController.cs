using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sprint4.Backend.Application.Common.Constants;
using Sprint4.Backend.Application.Features.Auth.Commands.Login;
using Sprint4.Backend.Application.Features.Auth.Commands.Logout;

namespace Sprint4.Backend.API.Controllers
{
    /// <summary>
    /// Expone los endpoints HTTP de autenticación (US01 y US02).
    /// El controller NO contiene lógica de negocio: solo traduce la petición HTTP
    /// a un Command de MediatR (patrón CQRS) y traduce el resultado a una respuesta HTTP.
    /// Cualquier historia de usuario nueva relacionada con auth se agrega como un
    /// Command/Query nuevo, sin modificar los que ya existen (Open/Closed Principle).
    /// </summary>
    [ApiController]
    [Route(AppConstants.Routes.AuthBase)]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>US01: autenticación y mapeo de rol.</summary>
        [HttpPost(AppConstants.Routes.Login)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _mediator.Send(new LoginCommand(request.Username, request.Password));

            if (!result.Success)
            {
                // US01 - Escenario 2: credenciales incorrectas.
                return Unauthorized(new { message = result.ErrorMessage });
            }

            // US01 - Escenario 1: autenticación exitosa.
            return Ok(result);
        }

        /// <summary>US02: cierre de sesión.</summary>
        [HttpPost(AppConstants.Routes.Logout)]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
        {
            await _mediator.Send(new LogoutCommand(request.UserId));
            return NoContent();
        }
    }

    // DTOs de entrada. Viven junto al controller porque son un detalle de transporte HTTP,
    // no una regla de negocio (esa vive en LoginCommand/LoginResult, dentro de Application).
    public record LoginRequestDto(string Username, string Password);
    public record LogoutRequestDto(int UserId);
}
