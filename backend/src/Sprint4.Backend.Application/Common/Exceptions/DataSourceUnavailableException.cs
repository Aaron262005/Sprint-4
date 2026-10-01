namespace Sprint4.Backend.Application.Common.Exceptions
{
    /// <summary>
    /// Se lanza cuando un origen de datos externo (Fake Store API, base de datos, etc.)
    /// no responde o devuelve datos inválidos.
    /// Vive en Application para que los Handlers puedan manejar la falla SIN conocer
    /// el detalle técnico (HttpRequestException, timeout, JSON inválido...): ese detalle
    /// lo traduce Infrastructure a esta excepción (Dependency Inversion Principle).
    /// </summary>
    public class DataSourceUnavailableException : Exception
    {
        public DataSourceUnavailableException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
    }
}
