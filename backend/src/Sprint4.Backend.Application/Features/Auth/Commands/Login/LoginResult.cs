namespace Sprint4.Backend.Application.Features.Auth.Commands.Login
{
    /// <summary>
    /// DTO de salida del login. Desacopla lo que el frontend recibe de la entidad
    /// de dominio "User" — la contraseña, por ejemplo, jamás se expone aquí.
    /// </summary>
    public class LoginResult
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
        public string? ErrorMessage { get; set; }

        public static LoginResult Fail(string message) => new()
        {
            Success = false,
            ErrorMessage = message
        };

        public static LoginResult Ok(string token, int userId, string username, string role) => new()
        {
            Success = true,
            Token = token,
            UserId = userId,
            Username = username,
            Role = role
        };
    }
}
