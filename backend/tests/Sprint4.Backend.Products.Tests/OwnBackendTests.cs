using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using Sprint4.Backend.Application.Features.Products;
using Sprint4.Backend.Application.Features.Products.DTOs;
using Sprint4.Backend.Domain.Entities;
using Sprint4.Backend.Infrastructure.Persistence;
using Xunit;

namespace Sprint4.Backend.Products.Tests;

public sealed class OwnBackendTests
{
    private static CreateProductDto Input => new()
    {
        Title = "Cuaderno", Price = 25, Description = "Azul",
        Image = "https://example.com/cuaderno.jpg", Category = "Escuela"
    };

    [Fact]
    public async Task Endpoints_create_read_update_list_delete_without_external_provider()
    {
        var folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        try
        {
            using var factory = new OwnFactory(Path.Combine(folder, "products.json"));
            using var client = factory.Client();
            using var createdResponse = await client.PostAsJsonAsync("/products", Input);
            Assert.Equal(HttpStatusCode.Created, createdResponse.StatusCode);
            var created = (await createdResponse.Content.ReadFromJsonAsync<Product>())!;
            var path = $"/products/{created.Id}";
            Assert.Equal(created, await client.GetFromJsonAsync<Product>(path));
            var update = new UpdateProductDto { Title = "Nuevo", Price = 30, Description = Input.Description,
                Image = Input.Image, Category = Input.Category };
            using var updatedResponse = await client.PutAsJsonAsync(path, update);
            Assert.Equal(HttpStatusCode.OK, updatedResponse.StatusCode);
            Assert.Equal("Nuevo", (await client.GetFromJsonAsync<Product>(path))!.Title);
            Assert.Single((await client.GetFromJsonAsync<Product[]>("/products"))!);
            using var deleted = await client.DeleteAsync(path);
            Assert.Equal(HttpStatusCode.OK, deleted.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync(path)).StatusCode);
            Assert.Empty((await client.GetFromJsonAsync<Product[]>("/products"))!);
            Assert.Equal(HttpStatusCode.NotFound, (await client.PutAsJsonAsync(path, update)).StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, (await client.DeleteAsync(path)).StatusCode);
        }
        finally { if (Directory.Exists(folder)) Directory.Delete(folder, true); }
    }

    [Fact]
    public async Task Repository_preserves_changes_and_next_id_after_restart()
    {
        var folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var path = Path.Combine(folder, "products.json");
        try
        {
            int id;
            using (var first = new JsonProductRepository(path))
                id = (await first.CreateAsync(Input, default)).Id;
            using (var second = new JsonProductRepository(path))
            {
                Assert.NotNull(await second.GetAsync(id, default));
                await second.DeleteAsync(id, default);
            }
            using var third = new JsonProductRepository(path);
            Assert.Empty(await third.ListAsync(default));
            Assert.True((await third.CreateAsync(Input, default)).Id > id);
            await Assert.ThrowsAsync<ProductNotFoundException>(() => third.DeleteAsync(id, default));
        }
        finally { if (Directory.Exists(folder)) Directory.Delete(folder, true); }
    }

    [Fact]
    public async Task Concurrent_creations_do_not_lose_products_or_repeat_ids()
    {
        var folder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        try
        {
            using var repository = new JsonProductRepository(Path.Combine(folder, "products.json"));
            var products = await Task.WhenAll(Enumerable.Range(0, 20).Select(_ => repository.CreateAsync(Input, default)));
            Assert.Equal(20, products.Select(product => product.Id).Distinct().Count());
            Assert.Equal(20, (await repository.ListAsync(default)).Count);
        }
        finally { if (Directory.Exists(folder)) Directory.Delete(folder, true); }
    }

    private sealed class OwnFactory(string path) : WebApplicationFactory<Program>
    {
        private readonly string key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(48));
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("JwtSettings:Key", key);
            builder.UseSetting("Products:StoragePath", path);
        }
        public HttpClient Client()
        {
            var client = CreateClient(new WebApplicationFactoryClientOptions
                { BaseAddress = new Uri("https://localhost"), AllowAutoRedirect = false, HandleCookies = false });
            var token = new JwtSecurityToken(claims: [new Claim(ClaimTypes.Role, "Administrador")],
                expires: DateTime.UtcNow.AddMinutes(5), signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(token));
            return client;
        }
    }
}
