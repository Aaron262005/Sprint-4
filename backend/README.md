# Sprint4 – Backend (.NET Core 8, CQRS + MVVM-friendly, Clean Architecture)

## 1. Cómo levantar la solución en Visual Studio

1. Instala el SDK de **.NET 8** si no lo tienes.
2. Abre una terminal en la carpeta `Sprint4-Backend/` y ejecuta:
   ```bash
   bash crear-solucion.sh
   ```
   Esto genera `Sprint4.Backend.sln` y lo enlaza con los 4 proyectos.
3. Abre `Sprint4.Backend.sln` con Visual Studio 2022.
4. Marca **Sprint4.Backend.API** como proyecto de inicio (clic derecho → "Set as Startup Project").
5. Ejecuta (F5). Se abrirá Swagger en `https://localhost:7000/swagger`.
6. Prueba `POST /api/auth/login` con, por ejemplo:
   ```json
   { "username": "admin1", "password": "Admin123!" }
   ```
   Verás el token y el rol `Administrador`. Usuarios de prueba en
   `Sprint4.Backend.Infrastructure/Persistence/MockUserDataStore.cs`.

## 2. Por qué está organizado así (para el equipo)

La solución está dividida en 4 proyectos, cada uno con una única responsabilidad
(Single Responsibility Principle a nivel de proyecto):

```
Sprint4.Backend.Domain          -> Entidades y Enums puros. No depende de nada.
Sprint4.Backend.Application     -> Reglas de negocio, CQRS (Commands/Queries + Handlers),
                                    interfaces (contratos) y el ÚNICO archivo de constantes.
Sprint4.Backend.Infrastructure  -> Implementaciones concretas: el mock de "base de datos",
                                    el mapeo de roles, la generación de JWT.
Sprint4.Backend.API             -> Controllers + Program.cs (composición de dependencias).
```

La dependencia siempre apunta hacia adentro:
`API → Infrastructure → Application → Domain`.
**Application nunca depende de Infrastructure ni de API** — solo define interfaces
(`IUserRepository`, `IRoleMapper`, `ITokenService`) que Infrastructure implementa.
Esto es Dependency Inversion Principle: si mañana cambian de JWT a otro esquema,
o de datos mock a la Fake Store API real, **solo se toca Infrastructure y una línea
en Program.cs** — Application y API no se modifican.

### CQRS con MediatR

Cada caso de uso es un `Command` (o `Query`, para lecturas) + su `Handler`, en su
propia carpeta bajo `Application/Features/<Módulo>/Commands|Queries/<Acción>/`.
Ejemplo ya implementado: `Features/Auth/Commands/Login/` (US01) y
`Features/Auth/Commands/Logout/` (US02).

**Para el equipo:** cuando implementen su historia de usuario, sigan el mismo patrón.
Ejemplo para US03 (catálogo):
```
Application/Features/Products/Queries/GetAllProducts/
    GetAllProductsQuery.cs
    GetAllProductsQueryHandler.cs
    ProductDto.cs
```
Y un `ProductsController` delgado que solo haga `_mediator.Send(new GetAllProductsQuery())`.

### Constantes

Todo literal fijo (rutas, mensajes, rangos de roles, nombres de configuración) vive en
`Application/Common/Constants/AppConstants.cs`. Si necesitan una constante nueva para
su historia de usuario, **agréguenla ahí**, no la escriban suelta en su código.

### Switch en vez de if/else

`RoleMapper` (Infrastructure/Services/RoleMapper.cs) usa un `switch` expression para
mapear Id → Rol, en vez de una cadena de `if/else if`. Si necesitan una decisión con
más de 2 casos, prefieran ese estilo — es más fácil de leer y de extender.

### Interfaces vs. clases abstractas

Se usaron **interfaces** (no clases abstractas) para los contratos de Infrastructure
(`IUserRepository`, `ITokenService`, `IRoleMapper`) porque:
- No comparten estado ni implementación por defecto entre sí.
- Una clase en C# solo puede heredar de una clase abstracta, pero puede implementar
  varias interfaces — más flexible para un proyecto en equipo donde cada quien agrega
  su propio contrato (`IProductRepository`, `ICartRepository`, etc.).

## 3. US01 y US02 — ya implementadas

- **US01 (Login):** `AuthController.Login` → `LoginCommand` → `LoginCommandHandler`.
  Cubre los 3 escenarios: éxito con mapeo de rol (200), credenciales inválidas (401),
  y la validación de conectividad se hace del lado del cliente (ver frontend).
- **US02 (Logout):** `AuthController.Logout` → `LogoutCommand` → `LogoutCommandHandler`.
  Con JWT no hay sesión que "cerrar" en el servidor; la limpieza real (token, rol,
  carrito, historial de pantallas) ocurre en el frontend — ver `AuthViewModel.logout()`.

## 4. Usuarios de prueba (mock)

| Username  | Password     | Id | Rol           |
|-----------|--------------|----|---------------|
| admin1    | Admin123!    | 1  | Administrador |
| admin2    | Admin456!    | 2  | Administrador |
| auditor1  | Auditor123!  | 3  | Auditor       |
| cliente1  | Cliente123!  | 4  | Cliente       |
| cliente2  | Cliente456!  | 5  | Cliente       |
