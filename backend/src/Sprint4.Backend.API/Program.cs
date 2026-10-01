using Sprint4.Backend.Infrastructure.Persistence;
using Sprint4.Backend.Application.Common.Constants;
using Sprint4.Backend.API.Middleware;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Sprint4.Backend.Application.Common.Interfaces;
using Sprint4.Backend.Application.Features.Auth.Commands.Login;
using Sprint4.Backend.Infrastructure.Repositories;
using Sprint4.Backend.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------------------
// Inyección de dependencias (Dependency Inversion Principle).
// La API solo conoce INTERFACES; las implementaciones concretas viven en Infrastructure.
// Si el equipo cambia el origen de datos o el proveedor de tokens, esto es
// lo ÚNICO que se toca en todo el proyecto.
// ------------------------------------------------------------------
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleMapper, RoleMapper>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// CQRS: registra automáticamente todos los Command/Query Handlers de Application.
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly));

// La ruta se puede configurar sin cambiar el contrato del repositorio.
var productFile = builder.Configuration[AppConstants.Products.StoragePathKey]
    ?? Path.Combine(builder.Environment.ContentRootPath,
        AppConstants.Products.DataDirectory, AppConstants.Products.DataFile);
builder.Services.AddSingleton(_ => new JsonProductRepository(productFile));
builder.Services.AddSingleton<IProductReader>(services => services.GetRequiredService<JsonProductRepository>());
builder.Services.AddSingleton<IProductWriter>(services => services.GetRequiredService<JsonProductRepository>());
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS para que el frontend Angular (http://localhost:4200) pueda consumir la API.
builder.Services.AddCors(options =>
{
    options.AddPolicy(AppConstants.Hosting.CorsPolicy, policy =>
        policy.WithOrigins(builder.Configuration.GetSection(AppConstants.Hosting.AllowedOrigins)
            .Get<string[]>() ?? [AppConstants.Hosting.DefaultAngularOrigin])
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var jwtKey = builder.Configuration[AppConstants.Jwt.KeyPath];
if (string.IsNullOrWhiteSpace(jwtKey) || Encoding.UTF8.GetByteCount(jwtKey) < AppConstants.Jwt.MinimumKeyBytes)
    throw new InvalidOperationException(AppConstants.Jwt.MissingKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(AppConstants.Hosting.CorsPolicy);
app.UseMiddleware<ProductsMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Permite probar los endpoints con un servidor en memoria.
public partial class Program { }
