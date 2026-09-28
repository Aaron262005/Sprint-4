using Sprint4.Backend.Application.Common.Constants;
using Sprint4.Backend.Application.Common.Interfaces;
using Sprint4.Backend.Domain.Enums;

namespace Sprint4.Backend.Infrastructure.Services
{
    /// <summary>
    /// Única fuente de verdad sobre la regla "Id de usuario -> Rol" (nota de negocio de US01).
    /// Se usa un switch expression en vez de una cadena de if/else if: la lectura es
    /// inmediata y, si el negocio agrega un rango nuevo, es una sola línea nueva aquí.
    /// </summary>
    public class RoleMapper : IRoleMapper
    {
        public UserRole MapRole(int userId)
        {
            return userId switch
            {
                >= AppConstants.RoleMapping.AdminIdRangeStart and <= AppConstants.RoleMapping.AdminIdRangeEnd
                    => UserRole.Administrador,

                AppConstants.RoleMapping.AuditorId => UserRole.Auditor,

                _ => UserRole.Cliente
            };
        }
    }
}
