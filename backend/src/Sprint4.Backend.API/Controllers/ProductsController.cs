using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sprint4.Backend.Application.Common.Constants;
using Sprint4.Backend.Application.Features.Products.Common;
using Sprint4.Backend.Application.Features.Products.Queries.GetAllProducts;

namespace Sprint4.Backend.API.Controllers
{
    /// <summary>
    /// Expone los endpoints HTTP del catálogo de productos (US03).
    /// El controller NO contiene lógica de negocio: solo traduce la petición HTTP
    /// a una Query de MediatR (patrón CQRS) y el resultado a una respuesta HTTP.
    /// [Authorize]: solo usuarios autenticados (Administrador, Cliente o Auditor).
    ///
    /// TODO (equipo catálogo): US04 y US05 agregan aquí sus endpoints como Queries nuevas
    /// (ej. GET categories, GET category/{category}, GET {id}) sin modificar GetAll.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route(AppConstants.Routes.ProductsBase)]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>US03: catálogo general de productos.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllProductsQuery(), cancellationToken);

            if (!result.Success)
            {
                // US03 - Escenario 3: el origen de datos no respondió.
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = result.ErrorMessage });
            }

            // US03 - Escenario 1: catálogo obtenido correctamente.
            return Ok(result.Products);
        }
    }
}
