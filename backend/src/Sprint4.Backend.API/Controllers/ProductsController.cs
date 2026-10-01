using Sprint4.Backend.Application.Features.Products.Queries.ListProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sprint4.Backend.Application.Common.Constants;
using Sprint4.Backend.Application.Features.Products.Commands.CreateProduct;
using Sprint4.Backend.Application.Features.Products.Commands.UpdateProduct;
using Sprint4.Backend.Application.Features.Products.Commands.DeleteProduct;
using Sprint4.Backend.Application.Features.Products.DTOs;
using Sprint4.Backend.Application.Features.Products.Queries.GetProduct;

namespace Sprint4.Backend.API.Controllers;

[ApiController]
[Authorize]
[Route(AppConstants.Products.Route)]
public sealed class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
        => Ok(await mediator.Send(new ListProductsQuery(), cancellationToken));

    [HttpGet(AppConstants.Products.ItemRoute)]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var product = await mediator.Send(new GetProductQuery(id), cancellationToken);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = AppConstants.Products.AdminRole)]
    public async Task<IActionResult> Create(CreateProductDto product, CancellationToken cancellationToken)
    {
        var created = await mediator.Send(new CreateProductCommand(product), cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut(AppConstants.Products.ItemRoute)]
    [Authorize(Roles = AppConstants.Products.AdminRole)]
    public async Task<IActionResult> Update(int id, UpdateProductDto product, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new UpdateProductCommand(id, product), cancellationToken));

    [HttpDelete(AppConstants.Products.ItemRoute)]
    [Authorize(Roles = AppConstants.Products.AdminRole)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        => Ok(await mediator.Send(new DeleteProductCommand(id), cancellationToken));
}
