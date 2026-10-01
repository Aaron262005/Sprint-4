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
        public static class Hosting
        {
            public const string CorsPolicy = "AllowAngularApp";
            public const string AllowedOrigins = "Frontend:AllowedOrigins";
            public const string DefaultAngularOrigin = "http://localhost:4200";
        }

        public static class Products
        {
            public const string Route = "products";
            public const string ItemRoute = "{id:int}";
            public const string ProviderUrl = "https://fakestoreapi.com/";
            public const string AdminRole = "Administrador";
            public const string Required = "Este campo es obligatorio.";
            public const string InvalidPrice = "El precio debe ser un número mayor que cero.";
            public const string InvalidImage = "La imagen debe tener una URL HTTPS válida.";
            public const string InvalidId = "El identificador debe ser mayor que cero.";
            public const string NotFound = "No se encontró el producto.";
            public const string ProviderError = "No se pudo completar la solicitud. Intenta de nuevo.";
            public const string HttpsRequired = "Debes utilizar HTTPS.";
            public const string HttpsScheme = "https";
            public const string MinimumPrice = "0.01";
            public const string MaximumPrice = "999999999";
            public const int TimeoutSeconds = 20;
        }

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
            public const string KeyName = "Key";
            public const string KeyPath = "JwtSettings:Key";
            public const string ExpirationName = "ExpirationMinutes";
            public const int MinimumKeyBytes = 32;
            public const string MissingKey = "Configura JwtSettings__Key fuera del repositorio con una clave aleatoria de al menos 32 bytes.";
            public const int ExpirationMinutes = 60;
        }
    }
}
