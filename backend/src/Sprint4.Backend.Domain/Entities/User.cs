namespace Sprint4.Backend.Domain.Entities
{
    /// <summary>
    /// Representa a un usuario del sistema (Administrador, Cliente o Auditor).
    /// Esta clase simula el registro que en producción vendría de una base de datos
    /// real o de la Fake Store API. Al vivir en Domain, no depende de EF Core,
    /// de HTTP ni de ningún detalle técnico (Dependency Inversion Principle).
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;

        // NOTA: en un entorno real esto sería un hash (BCrypt/Argon2), nunca texto plano.
        public string Password { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
