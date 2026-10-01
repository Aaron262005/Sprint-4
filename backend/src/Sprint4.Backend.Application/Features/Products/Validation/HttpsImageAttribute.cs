using System.ComponentModel.DataAnnotations;
using Sprint4.Backend.Application.Common.Constants;

namespace Sprint4.Backend.Application.Features.Products.Validation;

public sealed class HttpsImageAttribute : ValidationAttribute
{
    public HttpsImageAttribute() : base(AppConstants.Products.InvalidImage) { }

    public override bool IsValid(object? value) => value is string text
        && Uri.TryCreate(text, UriKind.Absolute, out var uri)
        && uri.Scheme == AppConstants.Products.HttpsScheme
        && !string.IsNullOrWhiteSpace(uri.Host);
}
