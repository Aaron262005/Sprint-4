namespace Sprint4.Backend.Domain.Enums
{
    /// <summary>
    /// Roles soportados por la aplicación (US01).
    /// Definidos en un único lugar: si el negocio agrega un rol nuevo,
    /// solo se agrega un valor aquí y en el RoleMapper (Infrastructure/Services/RoleMapper.cs).
    /// </summary>
    public enum UserRole
    {
        Administrador = 1,
        Cliente = 2,
        Auditor = 3
    }
}
