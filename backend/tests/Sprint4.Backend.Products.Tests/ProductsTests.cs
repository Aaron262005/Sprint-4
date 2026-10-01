using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sprint4.Backend.Application.Common.Interfaces;
using Sprint4.Backend.Application.Features.Products.DTOs;
using Sprint4.Backend.Domain.Entities;
using Xunit;

namespace Sprint4.Backend.Products.Tests;

public sealed class ProductsTests
{
    private static readonly CreateProductDto Valid = new()
    {
        Title = "Cuaderno", Price = 25, Description = "Cuaderno azul",
        Image = "https://example.com/cuaderno.jpg", Category = "Escuela"
    };

    [Theory]
    [InlineData("POST", "/products")]
    [InlineData("PUT", "/products/1")]
    [InlineData("DELETE", "/products/1")]
    public async Task Anonymous_cannot_write(string method, string path)
    {
        using var factory = new Factory();
        using var client = factory.Client();
        using var response = await client.SendAsync(Request(method, path));
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal(0, factory.Store.Writes);
    }

    [Theory]
    [InlineData("Cliente", "POST", "/products")]
    [InlineData("Cliente", "PUT", "/products/1")]
    [InlineData("Cliente", "DELETE", "/products/1")]
    [InlineData("Auditor", "POST", "/products")]
    [InlineData("Auditor", "PUT", "/products/1")]
    [InlineData("Auditor", "DELETE", "/products/1")]
    public async Task Other_roles_cannot_write(string role, string method, string path)
    {
        using var factory = new Factory();
        using var client = factory.Client(role);
        using var response = await client.SendAsync(Request(method, path));
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        Assert.Equal(0, factory.Store.Writes);
    }

    [Theory]
    [InlineData("POST", "/products", HttpStatusCode.Created)]
    [InlineData("PUT", "/products/1", HttpStatusCode.OK)]
    [InlineData("DELETE", "/products/1", HttpStatusCode.OK)]
    public async Task Administrator_can_write(string method, string path, HttpStatusCode expected)
    {
        using var factory = new Factory();
        using var client = factory.Client("Administrador");
        using var response = await client.SendAsync(Request(method, path));
        Assert.Equal(expected, response.StatusCode);
        Assert.Equal(1, factory.Store.Writes);
        Assert.True((await response.Content.ReadFromJsonAsync<Product>())!.Id > 0);
    }

    [Theory]
    [InlineData("   ", 25, "https://example.com/a.jpg")]
    [InlineData("Cuaderno", -1, "https://example.com/a.jpg")]
    [InlineData("Cuaderno", 25, "http://example.com/a.jpg")]
    public async Task Invalid_fields_never_reach_provider(string title, decimal price, string image)
    {
        using var factory = new Factory();
        using var client = factory.Client("Administrador");
        using var response = await client.PostAsJsonAsync("/products", Valid with { Title = title, Price = price, Image = image });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(0, factory.Store.Writes);
    }

    [Fact]
    public async Task Http_is_rejected_before_writing()
    {
        using var factory = new Factory();
        using var client = factory.Client("Administrador", "http://localhost");
        using var response = await client.PostAsJsonAsync("/products", Valid);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(0, factory.Store.Writes);
    }

    [Fact]
    public async Task Detail_is_available_to_authenticated_reader()
    {
        using var factory = new Factory();
        using var client = factory.Client("Cliente");
        var product = await client.GetFromJsonAsync<Product>("/products/1");
        Assert.Equal("Cuaderno", product!.Title);
    }

    private static HttpRequestMessage Request(string method, string path) => new(new HttpMethod(method), path)
    { Content = method == "DELETE" ? null : JsonContent.Create(Valid) };

    private sealed class Factory : WebApplicationFactory<Program>
    {
        private readonly string key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(48));
        public readonly Store Store = new();
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("JwtSettings:Key", key);
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IProductReader>(Store);
                services.AddSingleton<IProductWriter>(Store);
            });
        }
        public HttpClient Client(string? role = null, string url = "https://localhost")
        {
            var client = CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = new Uri(url), AllowAutoRedirect = false, HandleCookies = false });
            if (role is null) return client;
            var jwt = new JwtSecurityToken(claims: [new Claim(ClaimTypes.Role, role)], expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(jwt));
            return client;
        }
    }
    private sealed class Store : IProductReader, IProductWriter
    {
        public int Writes { get; private set; }
        public Task<Product?> GetAsync(int id, CancellationToken ct) => Task.FromResult<Product?>(Product(id));
        public Task<Product> CreateAsync(CreateProductDto p, CancellationToken ct) { Writes++; return Task.FromResult(Product(21)); }
        public Task<Product> UpdateAsync(int id, UpdateProductDto p, CancellationToken ct) { Writes++; return Task.FromResult(Product(id)); }
        public Task<Product> DeleteAsync(int id, CancellationToken ct) { Writes++; return Task.FromResult(Product(id)); }
        private static Product Product(int id) => new(id, "Cuaderno", 25, "Cuaderno azul", "https://example.com/a.jpg", "Escuela");
    }
}
