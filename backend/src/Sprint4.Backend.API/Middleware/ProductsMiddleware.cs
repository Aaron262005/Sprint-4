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
        catch (HttpRequestException)
        { await Error(context, StatusCodes.Status502BadGateway, AppConstants.Products.ProviderError); }
        catch (JsonException)
        { await Error(context, StatusCodes.Status502BadGateway, AppConstants.Products.ProviderError); }
        catch (OperationCanceledException) when (!context.RequestAborted.IsCancellationRequested)
        { await Error(context, StatusCodes.Status504GatewayTimeout, AppConstants.Products.ProviderError); }
    }

    private static Task Error(HttpContext context, int status, string message)
    {
        context.Response.StatusCode = status;
        return context.Response.WriteAsJsonAsync(new ProblemDetails { Status = status, Title = message });
    }
}
