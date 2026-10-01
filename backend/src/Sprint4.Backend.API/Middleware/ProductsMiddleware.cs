using Sprint4.Backend.Application.Features.Products;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Sprint4.Backend.Application.Common.Constants;

namespace Sprint4.Backend.API.Middleware;

// Solo afecta /products: no modifica los errores de otras historias.
public sealed class ProductsMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments($"/{AppConstants.Products.Route}"))
        {
            await next(context);
            return;
        }
        if (!context.Request.IsHttps)
        {
            await Error(context, StatusCodes.Status400BadRequest, AppConstants.Products.HttpsRequired);
            return;
        }
        try { await next(context); }
        catch (ValidationException exception)
        { await Error(context, StatusCodes.Status400BadRequest, exception.Message); }
        catch (ProductNotFoundException exception)
        { await Error(context, StatusCodes.Status404NotFound, exception.Message); }
        catch (IOException)
        { await Error(context, StatusCodes.Status500InternalServerError, AppConstants.Products.StorageError); }
        catch (UnauthorizedAccessException)
        { await Error(context, StatusCodes.Status500InternalServerError, AppConstants.Products.StorageError); }
        catch (JsonException)
        { await Error(context, StatusCodes.Status500InternalServerError, AppConstants.Products.StorageError); }
    }

    private static Task Error(HttpContext context, int status, string message)
    {
        context.Response.StatusCode = status;
        return context.Response.WriteAsJsonAsync(new ProblemDetails { Status = status, Title = message });
    }
}
