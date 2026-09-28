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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS para que el frontend Angular (http://localhost:4200) pueda consumir la API.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var jwtKey = builder.Configuration["JwtSettings:Key"] ?? "clave-temporal-de-desarrollo-cambiar-en-produccion";

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

app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
