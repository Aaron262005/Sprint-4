namespace Sprint4.Backend.Application.Common.Constants
{
    /// <summary>
    /// ÚNICO archivo de constantes de todo el backend.
    /// Regla del equipo: ningún literal "mágico" (ruta, mensaje, rango de roles,
    /// nombre de sección de configuración) se escribe suelto en el código.
    /// Todo valor fijo se declara aquí, así un cambio futuro se hace en un solo archivo.
    /// </summary>
    public static class AppConstants
    {
        /// <summary>Rutas HTTP expuestas por los controllers.</summary>
        public static class Routes
        {
            public const string AuthBase = "api/auth";
            public const string Login = "login";
            public const string Logout = "logout";
        }

        /// <summary>
        /// Regla de negocio de US01: a qué rol corresponde cada rango de Id de usuario.
        /// Usada por RoleMapper (Infrastructure/Services/RoleMapper.cs).
        /// </summary>
        public static class RoleMapping
        {
            public const int AdminIdRangeStart = 1;
            public const int AdminIdRangeEnd = 2;
            public const int AuditorId = 3;
            // Cualquier Id fuera de estos rangos se asigna como Cliente.
        }

        /// <summary>Mensajes de error mostrados al usuario (US01 escenario 2, US02).</summary>
        public static class ErrorMessages
        {
            public const string InvalidCredentials = "Usuario o contraseña inválidos";
            public const string UserNotFound = "El usuario no existe";
        }

        /// <summary>Claves de configuración usadas para generar el token de sesión.</summary>
        public static class Jwt
        {
            public const string SectionName = "JwtSettings";
            public const int ExpirationMinutes = 60;
        }
    }
}
